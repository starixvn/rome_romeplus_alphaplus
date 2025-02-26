<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:output method="html" indent="no" />
  <xsl:template match="root">
    <table class="tblDesc">
      <xsl:if test="betsetting/r">
        <tr class="headerDetail" onclick="toggle(this)">
          <td colspan="6">
            <span class="icon-chevron-down"></span>
            <xsl:value-of select="xslLogFormat:GetLogResource('BetSetting')" />
          </td>
        </tr>
        <tr>
          <td colspan="6">
            <table class="tblDesc">
              <tr class="headerSportTypeDetail" style="text-align:center;">
                <td>
                </td>
                <td>
                  <xsl:value-of select="xslLogFormat:GetLogResource('MinBet')" />
                </td>
                <td>
                  <xsl:value-of select="xslLogFormat:GetLogResource('MaxBet')" />
                </td>
                <td>
                  <xsl:value-of select="xslLogFormat:GetLogResource('MaxPerMatch')" />
                </td>
                <td>
                  <xsl:value-of select="xslLogFormat:GetLogResource('MaxPayoutPerMatch')" />
                </td>
              </tr>
              <xsl:for-each select="betsetting/r">
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetSportsBookBetSettingName(@sporttype,@bettype)" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@minbet" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@maxbet" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@mpm" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@maxpayout" />
                  </td>
                </tr>
              </xsl:for-each>
            </table>
          </td>
        </tr>
      </xsl:if>
      <xsl:if test="parlaymaxpayout/r">
        <tr class="headerDetail" onclick="toggle(this)">
          <td colspan="6">
            <span class="icon-chevron-down"></span>
            <xsl:value-of select="xslLogFormat:GetLogResource('ParlayMaxPayoutPerBet')" />
          </td>
        </tr>
        <tr>
          <td colspan="6">
            <table class="tblDesc">
              <tr class="headerSportTypeDetail" style="text-align:center;">
                <td></td>
                <td>
                  <xsl:value-of select="xslLogFormat:GetLogResource('SingleMatchParlay')" />
                </td>
                <td>
                  <xsl:value-of select="xslLogFormat:GetLogResource('Combo2')" />
                </td>
                <td>
                  <xsl:value-of select="xslLogFormat:GetLogResource('Combo35')" />
                </td>
                <td>
                  <xsl:value-of select="xslLogFormat:GetLogResource('ComboOver6')" />
                </td>
              </tr>
              <xsl:for-each select="parlaymaxpayout/r">
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetParlayMaxPayoutSportBetTypeName(@sporttype,@bettype)" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="xslLogFormat:FormatParlayMaxPayout(@singlemp)" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="xslLogFormat:FormatParlayMaxPayout(@combo2)" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="xslLogFormat:FormatParlayMaxPayout(@combo35)" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="xslLogFormat:FormatParlayMaxPayout(@combo6)" />
                  </td>
                </tr>
              </xsl:for-each>
            </table>
          </td>
        </tr>
      </xsl:if>
    </table>
  </xsl:template>
</xsl:stylesheet>