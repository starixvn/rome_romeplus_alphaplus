<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:output method="html" indent="no" />
  <xsl:template match="root">
    <table class="tblDesc">
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('subaccname')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@subaccname" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('ip')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="xslLogFormat:FormatIP(@ip)" />
        </td>
      </tr>
      
      <tr class="headerDetail" onclick="toggle(this)">
        <td colspan="4">
          <span class="icon-chevron-down"></span>
          <xsl:value-of select="xslLogFormat:GetLogResource('PotentialWinLimit')" />
        </td>
      </tr>
      
      <tr>
        <td colspan="8">
          <table class="tblDesc">
            <tr class="headerSportTypeDetail" style="text-align:center;">
              <td>
              </td>                
              <td>
                <xsl:value-of select="xslLogFormat:GetLogResource('WinLimit')" />
              </td>
              <td>
                <xsl:value-of select="xslLogFormat:GetLogResource('IsAutoReset')" />
              </td>                
            </tr>
            <xsl:for-each select="potentialwinlimit">
              <tr>
                <td class="colW1">
                  <xsl:value-of select="xslLogFormat:GetProductName(@productid)" />
                </td>
                <td class="colW2">
                  <xsl:value-of select="@winlimit" />
                </td>
                <td class="colW2">
                  <xsl:value-of select="@isautoreset" />
                </td>                  
              </tr>
            </xsl:for-each>
          </table>
        </td>
      </tr>           
    </table>
  </xsl:template>
</xsl:stylesheet>