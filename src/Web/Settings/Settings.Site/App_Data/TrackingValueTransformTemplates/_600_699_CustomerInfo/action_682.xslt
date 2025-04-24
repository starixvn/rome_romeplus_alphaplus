<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />

  <xsl:template match="chatbot_updateuserfunctions">
    <table class="tblDesc">
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('Outstanding')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@function1" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('TodayWinLoss')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@function2" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('BalanceInfo')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@function3" />
        </td>
      </tr>
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('YesterdayWinLoss')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@function4" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('PersonalMessage')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@function5" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('NormalAnnouncement')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@function7" />
        </td>
      </tr>
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('SpecialAnnouncement')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@function6" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('SystemAnnouncement')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@function8" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('CopyCustomer')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@function9" />
        </td>
      </tr>
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('ip')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="xslLogFormat:FormatIP(@ip)" />
        </td>
        <td class="colW1">
        </td>
        <td class="colW2">
        </td>
        <td class="colW1">
        </td>
        <td class="colW2">
        </td>
      </tr>
    </table>
  </xsl:template>
</xsl:stylesheet>