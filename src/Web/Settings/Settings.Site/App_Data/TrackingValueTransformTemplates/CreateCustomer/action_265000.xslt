<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:output method="html" indent="no" />
  <xsl:template match="root">
    <table class="tblDesc">      
        <tr class="headerDetail" onclick="toggle(this)">
          <td colspan="3">
            <span class="icon-chevron-down"></span>
            <xsl:value-of select="xslLogFormat:GetLogResource('PotentialWinLimit')" />
          </td>
        </tr>
        <tr>
          <td colspan="6">
            <table class="tblDesc">
              <tr class="headerSportTypeDetail" style="text-align:center;">
                <td></td>
                <td>
                  <xsl:value-of select="xslLogFormat:GetLogResource('WinLimit')" />
                </td>
                <td>
                  <xsl:value-of select="xslLogFormat:GetLogResource('IsAutoReset')" />
                </td>                
              </tr>
              <xsl:for-each select="winlimit">
                <xsl:for-each select="r">
                  <tr>
                    <td class="colW1">
                      <xsl:if test="@productid">
                        <xsl:value-of select="xslLogFormat:GetProductName(@productid)" />
                      </xsl:if>                      
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="xslLogFormat:FormatWinLossLimit(@winlimit)" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@isautoreset" />
                    </td>                  
                  </tr>
                </xsl:for-each>
              </xsl:for-each>         
            </table>
          </td>
        </tr>
    </table>
  </xsl:template>
</xsl:stylesheet>