<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />

  <xsl:template match="acc_cashservice_bankaccountagent_del">
    <xsl:choose>
      <xsl:when test="@custid">
        <table class="tblDesc">
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('username')" /></td>
            <td class="colW2">
              <xsl:value-of select="@username" />
            </td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('bank')" /></td>
            <td class="colW2">
              <xsl:value-of select="@bank" />
            </td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('accnumber')" /></td>
            <td class="colW2">
              <xsl:value-of select="@accnumber" />
            </td>
          </tr>
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('AgentBankAccount')" /></td>
            <td class="colW2">
              <xsl:value-of select="@agentbankaccount" />
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
          <tr>
            <td class="colW1">
                <xsl:value-of select="xslLogFormat:GetLogResource('ErrCode')" />
            </td>
            <td class="colW2">
                <xsl:value-of select="@errcode" />
            </td>
            <td class="colW1">
                <xsl:value-of select="xslLogFormat:GetLogResource('ErrMsg')" />
            </td>
            <td class="colW2">
                <xsl:value-of select="@errmsg" />
            </td>
            <td class="colW1"></td>
            <td class="colW2"></td>
          </tr>
        </table>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>
