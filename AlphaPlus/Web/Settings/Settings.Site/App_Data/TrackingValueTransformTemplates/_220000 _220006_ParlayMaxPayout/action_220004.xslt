<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />

  <xsl:template match="acc_custsetting_maxpayoutupd">
    <div class="headerSportTypeDetail">
      <xsl:value-of select="xslLogFormat:GetLogResource('UpdateParlayMaxPayout')" />
    </div>
    <table class="tblDesc">
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('subaccname')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@subaccname" />
        </td>

        <xsl:choose>
          <xsl:when test="@soccersinglematchparlay">
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('SoccerSMP')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@soccersinglematchparlay" />
            </td>
          </xsl:when>
          <xsl:otherwise>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('MixSportsSMP')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@mixsportssinglematchparlay" />
            </td>
          </xsl:otherwise>
        </xsl:choose>

        <xsl:choose>
          <xsl:when test="@soccercombo2">
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('Soccer2')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@soccercombo2" />
            </td>
          </xsl:when>
          <xsl:otherwise>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('MixSport2')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@mixsportscombo2" />
            </td>
          </xsl:otherwise>
        </xsl:choose>
      </tr>

      <tr>
        <xsl:choose>
          <xsl:when test="@soccercombo35">
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('Soccer35')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@soccercombo35" />
            </td>
          </xsl:when>
          <xsl:otherwise>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('MixSports35')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@mixsportscombo35" />
            </td>
          </xsl:otherwise>
        </xsl:choose>

        <xsl:choose>
          <xsl:when test="@soccercomboover6">
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('Soccer6pSP')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@soccercomboover6" />
            </td>
          </xsl:when>
          <xsl:otherwise>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('MixSports6pSP')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@mixsportscomboover6" />
            </td>
          </xsl:otherwise>
        </xsl:choose>

        <xsl:choose>
          <xsl:when test="not(@soccersinglematchparlay) or not(@mixsportssinglematchparlay)">
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('ErrCode')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@errcode" />
            </td>
          </xsl:when>
          <xsl:otherwise>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('MixSportsSMP')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@mixsportssinglematchparlay" />
            </td>
          </xsl:otherwise>
        </xsl:choose>
      </tr>

      <tr>
        <xsl:choose>
          <xsl:when test="not(@soccercombo2) or not(@mixsportscombo2)">
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('ErrMsg')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@errmsg" />
            </td>
          </xsl:when>
          <xsl:otherwise>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('MixSport2')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@mixsportscombo2" />
            </td>
          </xsl:otherwise>
        </xsl:choose>

        <xsl:choose>
          <xsl:when test="not(@soccercombo35) or not(@mixsportscombo35)">
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('IP')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="xslLogFormat:FormatIP(@ip)" />
            </td>
          </xsl:when>
          <xsl:otherwise>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('MixSports35')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@mixsportscombo35" />
            </td>
          </xsl:otherwise>
        </xsl:choose>

        <xsl:choose>
          <xsl:when test="not(@soccercomboover6) or not(@mixsportscomboover6)">
            <td class="colW1"></td>
            <td class="colW2"></td>
          </xsl:when>
          <xsl:otherwise>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('MixSports6pSP')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@mixsportscomboover6" />
            </td>
          </xsl:otherwise>
        </xsl:choose>
      </tr>

      <xsl:if test="@soccersinglematchparlay and @mixsportssinglematchparlay">
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

          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('IP')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="xslLogFormat:FormatIP(@ip)" />
          </td>
        </tr>
      </xsl:if>
    </table>
  </xsl:template>
</xsl:stylesheet>