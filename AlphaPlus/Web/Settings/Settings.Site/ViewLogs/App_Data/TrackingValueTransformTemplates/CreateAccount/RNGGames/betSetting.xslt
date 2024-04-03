<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:output method="html" indent="no" />
  <xsl:template match="betsetting[@grouptype=154]">
    <xsl:if test="r">
      <tr class="headerDetail" onclick="toggle(this)">
        <td colspan="6">
          <span class="icon-chevron-down"></span>
          <xsl:value-of select="xslLogFormat:GetLogResource('BetSetting')" />
        </td>
      </tr>
      <tr>
        <td colspan="6">
          <table class="tblDesc">
            <tr style="text-align:center;" class="headerSportTypeDetail">
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
                <xsl:value-of select="xslLogFormat:GetLogResource('MaxPerBall')" />
              </td>
            </tr>
            <xsl:for-each select="r">
              <tr>
                <td class="colW1">
                  <xsl:value-of select="xslLogFormat:GetRNGGamesBetSettingName(@sporttype,@bettype)" />
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
                  <xsl:value-of select="@maxpernumber" />
                </td>
              </tr>
            </xsl:for-each>
          </table>
        </td>
      </tr>
    </xsl:if>
  </xsl:template>
</xsl:stylesheet>