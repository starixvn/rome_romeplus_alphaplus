<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:output method="html" indent="no" />
  <xsl:template match="discount[@grouptype=156]">
    <xsl:if test="r">
      <tr class="headerDetail" onclick="toggle(this)">
        <td colspan="6">
          <span class="icon-chevron-down"></span>
          <xsl:value-of select="xslLogFormat:GetLogResource('Commision')" />
        </td>
      </tr>
      <tr>
        <td colspan="6">
          <table class="tblDesc tbl-last">
            <tr>
              <xsl:for-each select="r[position() &lt; 4]">
                <td class="colW1">
                  <xsl:value-of select="xslLogFormat:GetSportName(@sporttype)" />
                </td>
                <td class="colW2">
                  <xsl:value-of select="@playerdiscount" />
                </td>
              </xsl:for-each>
            </tr>
            <tr>
              <xsl:for-each select="r[position() &gt;= 4 and position() &lt; 7]">
                <td class="colW1">
                  <xsl:value-of select="xslLogFormat:GetSportName(@sporttype)" />
                </td>
                <td class="colW2">
                  <xsl:value-of select="@playerdiscount" />
                </td>
              </xsl:for-each>
            </tr>
            <tr>
              <xsl:for-each select="r[position() &gt;= 7]">
                <td class="colW1">
                  <xsl:value-of select="xslLogFormat:GetSportName(@sporttype)" />
                </td>
                <td class="colW2">
                  <xsl:value-of select="@discount" />
                </td>
                <xsl:if test="position()=last() and position() mod 3 &gt; 0">
                  <td class="colW1">
                  </td>
                  <td class="colW2">
                  </td>
                </xsl:if>
              </xsl:for-each>
            </tr>
          </table>
        </td>
      </tr>
    </xsl:if>
  </xsl:template>
</xsl:stylesheet>