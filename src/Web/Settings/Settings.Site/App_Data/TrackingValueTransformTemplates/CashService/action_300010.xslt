<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />

  <xsl:template match="age_cashservice_custservicetypeinitialize">
    <xsl:choose>
      <xsl:when test="@custid">
        <table class="tblDesc">
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('Contact')" /></td>
            <td class="colW2"><xsl:value-of select="@contact" /></td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('StartTime')" /></td>
            <td class="colW2"><xsl:value-of select="@starttime" /></td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('TrialEndTime')" /></td>
            <td class="colW2"><xsl:value-of select="@trialendtime" /></td>
          </tr>
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('DailyWithdrawalCount')" /></td>
            <td class="colW2"><xsl:value-of select="@dailywithdrawalcount" /></td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('SettingInfo')" /></td>
            <td class="colW2"><xsl:value-of select="xslLogFormat:GetTrackingRegisterInfo(@settinginfo)" /></td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('IP')" /></td>
            <td class="colW2"><xsl:value-of select="xslLogFormat:FormatIP(@ip)" /></td>
          </tr>
        </table>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>
