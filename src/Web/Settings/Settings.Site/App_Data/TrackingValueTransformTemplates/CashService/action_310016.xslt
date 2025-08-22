<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />

  <xsl:template match="r">
    <xsl:choose>
      <xsl:when test="@custid">
        <table class="tblDesc">
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('TransID')" /></td>
            <td class="colW2"><xsl:value-of select="@transid" /></td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('Status')" /></td>
            <td class="colW2"><xsl:value-of select="xslLogFormat:GetTrackingTransactionStatus(@statusid)" /></td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('OperatedBy')" /></td>
            <td class="colW2"><xsl:value-of select="xslLogFormat:GetLogResource('system')" /></td>
          </tr>
        </table>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>
