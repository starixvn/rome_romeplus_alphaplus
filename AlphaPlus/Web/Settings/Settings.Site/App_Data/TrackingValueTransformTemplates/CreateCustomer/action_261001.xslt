<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:output method="html" indent="no" />
  <xsl:template match="root/discount">
    <table class="tblDesc">
      <tr>
        <td colspan="6">
          <table class="tblDesc">
            <tr>
              <xsl:apply-templates select="r[position() &lt; 4]" />
            </tr>
            <tr>
              <xsl:apply-templates select="r[position() &gt;= 4]" />
            </tr>
          </table>
        </td>
      </tr>
    </table>
  </xsl:template>
  <xsl:template match="discount/r">
    <td class="colW1">
      <xsl:value-of select="xslLogFormat:GetSportbookCommissionName(@sporttype,@bettype)" />
    </td>
    <td class="colW2">
      <xsl:value-of select="@discount" />
    </td>
  </xsl:template>
</xsl:stylesheet>