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
            <td class="colW2">
              <xsl:value-of select="@subaccname" />
            </td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('IP')" /></td>
            <td class="colW2">
              <xsl:value-of select="xslLogFormat:FormatIP(@ip)" />
            </td>
          </tr>
        </table>

        <table class="tblDesc">
          <xsl:for-each select="promotion">
              <tr>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('Promotion')" /></td>
                <td class="colW2"><xsl:value-of select="@promotionid" /></td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('Type ')" /></td>
                <td class="colW2"><xsl:value-of select="xslLogFormat:GetPromotionTypeName_OldNewValue(@promotiontype)" /></td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('Status')" /></td>
                <td class="colW2"><xsl:value-of select="xslLogFormat:GetStatus_OldNewValue(@enabled)" /></td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('From ')" /></td>
                <td class="colW2"><xsl:value-of select="@fromdate" /></td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('To')" /></td>
                <td class="colW2"><xsl:value-of select="@todate" /></td>
              </tr>
          </xsl:for-each>
        </table>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>
