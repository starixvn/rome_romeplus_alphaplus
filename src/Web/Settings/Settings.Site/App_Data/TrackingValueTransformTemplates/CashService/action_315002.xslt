<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />

  <xsl:template match="root">
    <xsl:choose>
      <xsl:when test="@custid">
        <table class="tblDesc">
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('SubAccName')" /></td>
            <td class="colW2"><xsl:value-of select="@subaccname" /></td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('IP')" /></td>
            <td class="colW2"><xsl:value-of select="xslLogFormat:FormatIP(@ip)" /></td>
            <td class="colW1"></td>
            <td class="colW2"></td>
          </tr>
        </table>

        <xsl:for-each select="promotion">
          <table class="tblDesc">
            <tr class="headerDetail" onclick="toggle(this)">
              <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('Promotion')" /></td>
              <td class="colW2" colspan="5"><xsl:value-of select="@promotionid" /></td>
            </tr>
            <xsl:for-each select="r">
              <tr>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('language')" /></td>
                <td class="colW2"><xsl:value-of select="@language" /></td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('Title')" /></td>
                <td class="colW2"><xsl:value-of select="@title" /></td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('Content')" /></td>
                <td class="colW2"><xsl:value-of select="@content" /></td>
              </tr>
            </xsl:for-each>
          </table>
        </xsl:for-each>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>
