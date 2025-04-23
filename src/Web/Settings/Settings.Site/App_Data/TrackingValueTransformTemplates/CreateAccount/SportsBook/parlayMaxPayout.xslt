<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:output method="html" indent="no" />
  <xsl:template match="parlaymaxpayout">
    <xsl:if test="r">
      <tr class="headerDetail" onclick="toggle(this)">
        <td colspan="6">
          <span class="icon-chevron-down"></span>
          <xsl:value-of select="xslLogFormat:GetLogResource('ParlayMaxPayoutPerBet')" />
        </td>
      </tr>
      <tr>
        <td colspan="6">
          <table class="tblDesc">
            <tr style="text-align:center;" class="headerSportTypeDetail">
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
            <xsl:for-each select="r">
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
  </xsl:template>
</xsl:stylesheet>