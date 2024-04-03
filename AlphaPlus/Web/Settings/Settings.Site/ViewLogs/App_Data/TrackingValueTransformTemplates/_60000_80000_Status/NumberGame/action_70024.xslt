<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />

  <xsl:template match="acc_adminupddisablenumbergame">
    <table class="tblDesc">
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('isDisabled')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@disablenumber" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('updateAllMembers')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@updateallmembers" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('isDisabledAllMembers')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@disableallmembers" />
        </td>
      </tr>

      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('ip')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@ip" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('errCode')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@errcode" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('errMsg')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@errmsg" />
        </td>
      </tr>
    </table>
  </xsl:template>
</xsl:stylesheet>
