<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="1"></xsl:param>
  <xsl:output method="html" indent="no" />

  <xsl:template match="acc_customerwinlimitupd">
    <xsl:if test="xslLogFormat:IsGoldDeluxe(@sporttype)">
      <table class="tblDesc">
        <tr>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('Sport')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="xslLogFormat:GetSportName(@sporttype)" />
          </td>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('MaximumWin')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="xslLogFormat:FormatMemberWinLimit(@maxwinlimit)" />
          </td>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('DailyAutoReset')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="@isautoreset" />
          </td>
        </tr>
        <tr>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('SubAccName')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="@subaccname" />
          </td>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('Language')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="@language" />
          </td>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('IP')" />
          </td>
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
          <td class="colW1"></td><td class="colW2"></td>
        </tr>
      </table>
    </xsl:if>
    <xsl:if test="xslLogFormat:IsRNGCasino(@sporttype) =  'True'">
      <table class="tblDesc">
        <tr>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('Sport')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="xslLogFormat:GetSportName(@sporttype)" />
          </td>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('MaximumWin')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="xslLogFormat:FormatMemberWinLimit(@maxwinlimit)" />
          </td>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('DailyAutoReset')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="@isautoreset" />
          </td>
        </tr>
        <tr>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('MaximumLoss')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="xslLogFormat:FormatMemberWinLimit(@maxlosslimit)" />
          </td>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('SubAccName')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="@subaccname" />
          </td>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('Language')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="@language" />
          </td>
        </tr>
        <tr>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('IP')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="xslLogFormat:FormatIP(@ip)" />
          </td>
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
        </tr>
      </table>
    </xsl:if>
  </xsl:template>

</xsl:stylesheet>