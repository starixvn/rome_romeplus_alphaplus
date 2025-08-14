<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />

  <xsl:template match="r">
    <xsl:choose>
      <xsl:when test="@custid">
        <table class="tblDesc">
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('CreatedTime')" /></td>
            <td class="colW2"><xsl:value-of select="@createdtime" /></td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('From')" /></td>
            <td class="colW2"><xsl:value-of select="@fromdate" /></td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('To')" /></td>
            <td class="colW2"><xsl:value-of select="@todate" /></td>
          </tr>
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('Title')" /></td>
            <td class="colW2"><xsl:value-of select="@title" /></td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('Amount ')" /></td>
            <td class="colW2"><xsl:value-of select="@amount" /></td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('Status')" /></td>
            <td class="colW2"><xsl:value-of select="@status" /></td>
          </tr>
        </table>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>
