<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />

  <xsl:template match="age_cashservice_bankaccountagent_upd">
    <xsl:choose>
      <xsl:when test="@custid">
        <table class="tblDesc">
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('bank')" /></td>
            <td class="colW2">
              <xsl:value-of select="xslLogFormat:GetBankName(@bankcode)" />
            </td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('status')" /></td>
            <td class="colW2">
              <xsl:value-of select="xslLogFormat:GetStatus(@bankaccountstatus)" />
            </td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('TransactionType')" /></td>
            <td class="colW2">
              <xsl:value-of select="xslLogFormat:GetTransactionTypes(@transtypelist)" />
            </td>
          </tr>
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('AccountHolderName')" /></td>
            <td class="colW2">
              <xsl:value-of select="@accountholdername" />
            </td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('BankAccountNumber')" /></td>
            <td class="colW2">
              <xsl:value-of select="@bankaccountnumber" />
            </td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('AccountNickname')" /></td>
            <td class="colW2">
              <xsl:value-of select="@accountnickname" />
            </td>
          </tr>
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('DepositMatchingMode')" /></td>
            <td class="colW2">
              <xsl:value-of select="xslLogFormat:GetDepositMatchingMode(@depositmatchingmode)" />
            </td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('PromptPay')" /></td>
            <td class="colW2">
              <xsl:value-of select="xslLogFormat:GetStatus(@promptpayenabled)" />
            </td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('Mobile')" /></td>
            <td class="colW2">
              <xsl:value-of select="@promptpaymobile" />
            </td>
          </tr>
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('SubAccName')" /></td>
            <td class="colW2">
              <xsl:value-of select="@subaccname" />
            </td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('IP')" /></td>
            <td class="colW2">
              <xsl:value-of select="xslLogFormat:FormatIP(@ip)" />
            </td>
            <td class="colW1"></td>
            <td class="colW2"></td>
          </tr>
        </table>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>
