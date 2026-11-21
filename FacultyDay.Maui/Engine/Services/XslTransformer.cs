using System.Text;
using System.Xml;
using System.Xml.Xsl;

namespace FacultyDay.Engine.Services;

public sealed class XslTransformer
{
    private const string EmbeddedXsl = "events.xsl";

    public string Transform(string xmlPath)
    {
        using var xsl = typeof(XslTransformer).Assembly.GetManifestResourceStream(EmbeddedXsl)
            ?? throw new InvalidOperationException($"Вбудований ресурс '{EmbeddedXsl}' не знайдено.");
        return Transform(xmlPath, xsl);
    }

    public string Transform(string xmlPath, Stream xslStream)
    {
        var xslt = new XslCompiledTransform();
        using (var xslReader = XmlReader.Create(xslStream))
            xslt.Load(xslReader);

        using var ms = new MemoryStream();
        using (var writer = XmlWriter.Create(ms, xslt.OutputSettings))
            xslt.Transform(xmlPath, writer);

        ms.Position = 0;
        using var reader = new StreamReader(ms, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);
        return reader.ReadToEnd();
    }
}
