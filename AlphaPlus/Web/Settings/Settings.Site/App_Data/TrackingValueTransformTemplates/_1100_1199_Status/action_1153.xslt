<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:output method="html" indent="no" />
  <xsl:template match="acc_custsetting_syncstatusupd">
    <table class="tblDesc">
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('subaccname')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@subaccname" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('IP')" />
        </td>
        <td  class="colW2">
          <xsl:value-of select="xslLogFormat:FormatIP(@ip)" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('language')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@language" />
        </td>
      </tr>
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('product')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="xslLogFormat:GetProductName(@productid)" />
        </td>
        <td class="colW1"></td>
        <td class="colW2"></td>
        <td class="colW1"></td>
        <td class="colW2"></td>
      </tr>
    </table>
    <xsl:for-each select="pt">
      <table  class="tblDesc">
        <tr>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('sport')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="xslLogFormat:GetSportName(@sporttype)" />
          </td>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('bettype')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="xslLogFormat:GetBetTypeName(@bettype)" />
          </td>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('lblPresetPTMaster')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="@issync" />
          </td>
        </tr>

      </table>
    </xsl:for-each>
  </xsl:template>
</xsl:stylesheet>
