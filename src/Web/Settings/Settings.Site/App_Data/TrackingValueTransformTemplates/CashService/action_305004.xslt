<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />

  <xsl:template match="age_cashservice_paymentmethod_config">
    <xsl:choose>
      <xsl:when test="@custid">
        <table class="tblDesc">
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('Provider')" /></td>
            <td class="colW2">
              <xsl:value-of select="@providername" />
            </td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('TransactionType')" /></td>
            <td class="colW2">
              <xsl:value-of select="xslLogFormat:GetTransactionTypeName_OldNewValue(@transactiontype)" />
            </td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('PaymentMethod')" /></td>
            <td class="colW2">
              <xsl:value-of select="xslLogFormat:GetPaymentMethodName_OldNewValue(@paymentmethodid)" />
            </td>
          </tr>
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('PaymentMethodStatus')" /></td>
            <td class="colW2">
              <xsl:value-of select="xslLogFormat:GetPaymentMethodStatusName_OldNewValue(@paymentmethodstatus)" />
            </td>
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
      </xsl:when>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>
