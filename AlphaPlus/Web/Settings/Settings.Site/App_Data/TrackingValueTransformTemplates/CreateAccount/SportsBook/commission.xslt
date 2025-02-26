<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:output method="html" indent="no" />
  <xsl:template match="discount[@grouptype=150]">
    <xsl:if test="r">
      <tr class="headerDetail" onclick="toggle(this)">
        <td colspan="6">
          <span class="icon-chevron-down"></span>
          <xsl:value-of select="xslLogFormat:GetLogResource('Commision')" />
        </td>
      </tr>
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
    </xsl:if>
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