using System.Text;
using System.Xml;
using System.Xml.Xsl;

namespace FacultyDay.Engine.Services;

/// <summary>Перетворює XML на HTML за допомогою XSL-документа.</summary>
public sealed class XslTransformer
{
    private const string EmbeddedXsl = "events.xsl";

    /// <summary>Трансформація зі стандартним XSL, вбудованим у збірку (Data/events.xsl).</summary>
    public string Transform(string xmlPath)
    {
        using var xsl = typeof(XslTransformer).Assembly.GetManifestResourceStream(EmbeddedXsl)
            ?? throw new InvalidOperationException($"Вбудований ресурс '{EmbeddedXsl}' не знайдено.");
        return Transform(xmlPath, xsl);
    }

    /// <summary>Трансформація з довільним XSL (наприклад, вибраним користувачем).</summary>
    public string Transform(string xmlPath, Stream xslStream)
    {
        var xslt = new XslCompiledTransform();
        using (var xslReader = XmlReader.Create(xslStream))
            xslt.Load(xslReader);

        // Пишемо в байти (UTF-8), а не в StringWriter: інакше в HTML потрапить charset=utf-16
        using var ms = new MemoryStream();
        using (var writer = XmlWriter.Create(ms, xslt.OutputSettings))
            xslt.Transform(xmlPath, writer);

        ms.Position = 0;
        using var reader = new StreamReader(ms, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);
        return reader.ReadToEnd();
    }
}
