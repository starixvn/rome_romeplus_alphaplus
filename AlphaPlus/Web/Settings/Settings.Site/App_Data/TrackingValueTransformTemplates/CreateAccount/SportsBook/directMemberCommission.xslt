<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:output method="html" indent="no" />
  <xsl:template match="discount[@grouptype=150]">
    <xsl:param name="uplineroleid" />
    <xsl:if test="r">
      <tr class="headerDetail" onclick="toggle(this)">
        <td colspan="6">
          <span class="icon-chevron-down"></span>
          <xsl:value-of select="xslLogFormat:GetLogResource('Commision')" />
        </td>
      </tr>
      <tr>
        <td colspan="6">
          <table class="tblDesc">
            <tr class="headerSportTypeDetail" style="text-align:center;">
              <td></td>
              <td>
                <xsl:choose>
                  <xsl:when test="$uplineroleid=4">
                    <xsl:value-of select="xslLogFormat:GetLogResource('CommForSuper')" />
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="xslLogFormat:GetLogResource('CommForMaster')" />
                  </xsl:otherwise>
                </xsl:choose>
              </td>
              <td>
                <xsl:value-of select="xslLogFormat:GetLogResource('CommForMember')" />
              </td>
            </tr>
            <xsl:for-each select="r">
              <tr>
                <td class="colW1">
                  <xsl:value-of select="xslLogFormat:GetSportbookMemberCommissionName(@sporttype,@bettype)" />
                </td>
                <td class="colW2">
                  <xsl:choose>
                    <xsl:when test="$uplineroleid=4">
                      <xsl:value-of select="@superdiscount" />
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="@masterdiscount" />
                    </xsl:otherwise>
                  </xsl:choose>
                </td>
                <td class="colW2">
                  <xsl:value-of select="@playerdiscount" />
                </td>
              </tr>
            </xsl:for-each>
          </table>
        </td>
      </tr>
    </xsl:if>
  </xsl:template>
</xsl:stylesheet>