<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />

  <xsl:template match="r">
    <xsl:choose>
      <xsl:when test="@custid">
        <table class="tblDesc">
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('creator')" /></td>
            <td class="colW2"><xsl:value-of select="@creator" /></td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('TransactionType')" /></td>
            <td class="colW2"><xsl:value-of select="xslLogFormat:GetTrackingTransactionTypeName(@transactiontypeid)" /></td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('PaymentMethod')" /></td>
            <td class="colW2"><xsl:value-of select="xslLogFormat:GetTrackingPaymentMethodName(@paymentmethod)" /></td>
          </tr>
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('From')" /></td>
            <td class="colW2"><xsl:value-of select="@paymentfrom" /></td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('To')" /></td>
            <td class="colW2"><xsl:value-of select="@paymentto" /></td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('Amount')" /></td>
            <td class="colW2"><xsl:value-of select="@amount" /></td>
          </tr>
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('transID')" /></td>
            <td class="colW2"><xsl:value-of select="@transid" /></td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('AdjustCreditAmt')" /></td>
            <td class="colW2"><xsl:value-of select="@adjustcreditamt" /></td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('Status')" /></td>
            <td class="colW2"><xsl:value-of select="xslLogFormat:GetTrackingTransactionStatus(@statusid)" /></td>
          </tr>
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('IP')" /></td>
            <td class="colW2"><xsl:value-of select="xslLogFormat:FormatIP(@ip)" /></td>
            <td class="colW1"></td>
            <td class="colW2"></td>
            <td class="colW1"></td>
            <td class="colW2"></td>
          </tr>
        </table>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>
