<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />

  <xsl:template match="r">
    <xsl:choose>
      <xsl:when test="@custid">
        <table class="tblDesc">
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('transID')" /></td>
            <td class="colW2">
              <xsl:value-of select="@transid" />
            </td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('PaymentMethod')" /></td>
            <td class="colW2">
              <xsl:value-of select="xslLogFormat:GetPaymentMethodName_OldNewValue(@paymenttype)" />
            </td>
          </tr>
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('From')" /></td>
            <td class="colW2">
              <xsl:value-of select="@paymentfrom" />
            </td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('IP')" /></td>
            <td class="colW2">
              <xsl:value-of select="xslLogFormat:FormatIP(@ip)" />
            </td>
          </tr>
        </table>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>
