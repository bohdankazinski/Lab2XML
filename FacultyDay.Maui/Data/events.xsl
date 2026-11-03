<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html" encoding="utf-8" indent="yes"/>

  <!-- Виводить значення або тире, якщо атрибута немає -->
  <xsl:template name="val">
    <xsl:param name="v"/>
    <xsl:choose>
      <xsl:when test="string($v)"><xsl:value-of select="$v"/></xsl:when>
      <xsl:otherwise>—</xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/FacultyDay">
    <html>
      <head>
        <title>День факультету</title>
        <style>
          body { font-family: Segoe UI, Arial, sans-serif; margin: 24px; color: #222; }
          h1 { margin-bottom: 4px; }
          .sub { color: #666; margin-bottom: 20px; }
          table { border-collapse: collapse; width: 100%; font-size: 14px; }
          th, td { border: 1px solid #ccc; padding: 6px 8px; vertical-align: top; text-align: left; }
          th { background: #2d5da8; color: #fff; }
          tr:nth-child(even) td { background: #f4f7fb; }
          ul { margin: 0; padding-left: 18px; }
        </style>
      </head>
      <body>
        <h1>День факультету</h1>
        <div class="sub">
          <xsl:value-of select="@faculty"/>, <xsl:value-of select="@year"/> ·
          заходів: <xsl:value-of select="count(Event)"/>
        </div>
        <table>
          <tr>
            <th>№</th><th>Назва заходу</th><th>Дата</th><th>Час</th><th>Місце</th>
            <th>Підготовка</th><th>Відповідальний</th><th>Доручення</th><th>Неформальна характеристика</th>
          </tr>
          <xsl:apply-templates select="Event">
            <xsl:sort select="@date"/>
            <xsl:sort select="@time"/>
          </xsl:apply-templates>
        </table>
      </body>
    </html>
  </xsl:template>

  <xsl:template match="Event">
    <tr>
      <td><xsl:value-of select="@id"/></td>
      <td><b><xsl:call-template name="val"><xsl:with-param name="v" select="@name"/></xsl:call-template></b></td>
      <td><xsl:call-template name="val"><xsl:with-param name="v" select="@date"/></xsl:call-template></td>
      <td><xsl:call-template name="val"><xsl:with-param name="v" select="@time"/></xsl:call-template></td>
      <td><xsl:call-template name="val"><xsl:with-param name="v" select="@place"/></xsl:call-template></td>
      <td>
        <xsl:choose>
          <xsl:when test="@prepFrom and @prepTo"><xsl:value-of select="@prepFrom"/> – <xsl:value-of select="@prepTo"/></xsl:when>
          <xsl:when test="@prepTo">до <xsl:value-of select="@prepTo"/></xsl:when>
          <xsl:when test="@prepFrom">з <xsl:value-of select="@prepFrom"/></xsl:when>
          <xsl:otherwise>—</xsl:otherwise>
        </xsl:choose>
      </td>
      <td>
        <xsl:value-of select="Responsible/@fullName"/><br/>
        <i><xsl:value-of select="Responsible/@position"/></i><br/>
        <xsl:value-of select="Responsible/@chair"/>
      </td>
      <td>
        <ul>
          <xsl:for-each select="Tasks/Task">
            <li><xsl:value-of select="@title"/> — <xsl:value-of select="@executor"/> (до <xsl:value-of select="@due"/>)</li>
          </xsl:for-each>
        </ul>
      </td>
      <td><xsl:call-template name="val"><xsl:with-param name="v" select="@note"/></xsl:call-template></td>
    </tr>
  </xsl:template>
</xsl:stylesheet>
