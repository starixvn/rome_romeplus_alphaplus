<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />
  <xsl:key name="template-by-group" match="r" use="@groupid"/>
  <xsl:template match="root">
    <table class="tblDesc">
      <tr class="headerDetail" style="text-align:center;">
        <td rowspan="2">
          <xsl:value-of select="xslLogFormat:GetLogResource('TemplateName')" />
        </td>
        <td style="text-align:center;">
          <xsl:attribute name="colspan">
            <xsl:value-of select="count(r)"/>
          </xsl:attribute>
          <xsl:value-of select="xslLogFormat:GetLogResource('SuperPT')" />
        </td>
        <td rowspan="2">
          <xsl:value-of select="xslLogFormat:GetLogResource('SelectedByDefault')" />
        </td>
        <td rowspan="2">
          <xsl:value-of select="xslLogFormat:GetLogResource('subaccname')" />
        </td>
        <td rowspan="2">
          <xsl:value-of select="xslLogFormat:GetLogResource('ip')" />
        </td>
      </tr>
      <tr class="headerSportTypeDetail border-last-item" style="text-align:center;">
        <xsl:for-each select="r[count(.|key('template-by-group',@groupid)[1])=1]">
            <xsl:choose>
              <xsl:when test="count(key('template-by-group', @groupid)) &gt; 1">
                <xsl:for-each select="key('template-by-group', @groupid)">
                  <td>
                    <xsl:value-of select="xslLogFormat:GetSubProductGroupName(@subgroupid)" />
                  </td>
                </xsl:for-each>
              </xsl:when>
              <xsl:otherwise>
                <td>
                  <xsl:value-of select="xslLogFormat:GetProductGroupName(@groupid)" />
                </td>
              </xsl:otherwise>
            </xsl:choose>
        </xsl:for-each>
      </tr>
      <tr>
        <td class="colW1">
          <xsl:value-of select="@templatename" />
        </td>
        <xsl:for-each select="r">
          <td class="colW2">
            <xsl:value-of select="@pt" />
          </td>
        </xsl:for-each>
        <td class="colW2">
          <xsl:value-of select="@isdefault" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@subaccname" />
        </td>
        <td class="colW2">
          <xsl:value-of select="xslLogFormat:FormatIP(@ip)" />
        </td>
      </tr>
    </table>
  </xsl:template>
</xsl:stylesheet>