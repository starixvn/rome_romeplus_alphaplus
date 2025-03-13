<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:output method="html" indent="no" />
  <xsl:key name="pt-by-sporttype" match="pt/r" use="@sporttype" />
  <xsl:template name="directMemberPT">
    <xsl:param name="groupid" />
    <xsl:param name="uplineroleid" />
    <xsl:if test="pt/r">
      <tr class="headerDetail" onclick="toggle(this)">
        <td colspan="6">
          <span class="icon-chevron-down"></span>
          <xsl:value-of select="xslLogFormat:GetLogResource('PositionTaking')" />
        </td>
      </tr>
      <tr>
        <td colspan="6">
          <table class="tblDesc tbl-last">
            <xsl:choose>
              <xsl:when test="$groupid=1">
                <xsl:for-each select="pt/r[count(. | key('pt-by-sporttype', @sporttype)[1]) = 1]">
                  <tr class="headerSportTypeDetail">
                    <td>
                      <xsl:value-of select="xslLogFormat:GetSportName(@sporttype)" />
                    </td>
                    <td style="text-align:center;">
                      <xsl:choose>
                        <xsl:when test="$uplineroleid=4">
                          <xsl:value-of select="xslLogFormat:GetLogResource('SuperPT')" />
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="xslLogFormat:GetLogResource('MasterPT')" />
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                  </tr>
                  <xsl:for-each select="key('pt-by-sporttype', @sporttype)">
                    <xsl:if test="@sporttype=99 and @bettype=9">
                      <tr class="headerSportTypeDetail">
                        <td>
                          <xsl:value-of select="xslLogFormat:GetSportsBookBetSettingName(@sporttype,@bettype)" />
                        </td>
                        <td style="text-align:center;">
                          <xsl:choose>
                            <xsl:when test="$uplineroleid=4">
                              <xsl:value-of select="xslLogFormat:GetLogResource('SuperPT')" />
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:value-of select="xslLogFormat:GetLogResource('MasterPT')" />
                            </xsl:otherwise>
                          </xsl:choose>
                        </td>
                      </tr>
                    </xsl:if>
                    <tr>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetBetTypeName(@bettype)" />
                      </td>
                      <td class="colW2">
                        <xsl:choose>
                          <xsl:when test="$uplineroleid=4">
                            <xsl:value-of select="@superpt" />
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:value-of select="@masterpt" />
                          </xsl:otherwise>
                        </xsl:choose>
                      </td>
                    </tr>
                  </xsl:for-each>
                </xsl:for-each>
              </xsl:when>
              <xsl:otherwise>
                <tr style="text-align:center;" class="headerSportTypeDetail">
                  <td></td>
                  <td>
                    <xsl:choose>
                      <xsl:when test="$uplineroleid=4">
                        <xsl:value-of select="xslLogFormat:GetLogResource('SuperPT')" />
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="xslLogFormat:GetLogResource('MasterPT')" />
                      </xsl:otherwise>
                    </xsl:choose>
                  </td>
                </tr>
                <xsl:for-each select="pt/r">
                  <tr>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetDisplayName(@settinglevel,@sporttype,@bettype,@productid)" />
                    </td>
                    <td class="colW2">
                      <xsl:choose>
                        <xsl:when test="$uplineroleid=4">
                          <xsl:value-of select="@superpt" />
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="@masterpt" />
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                  </tr>
                </xsl:for-each>
              </xsl:otherwise>
            </xsl:choose>
          </table>
        </td>
      </tr>
    </xsl:if>
  </xsl:template>
</xsl:stylesheet>