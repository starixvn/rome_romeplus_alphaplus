<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:output method="html" indent="no" />
  <xsl:key name="groupid" match="stakelevel" use="@groupid" />
  <!-- <xsl:include href="..\..\stringreplace.xslt" /> -->
  <xsl:template name="string-replace-all">
    <xsl:param name="text" />
    <xsl:param name="replace" />
    <xsl:param name="by" />
    <xsl:choose>
      <xsl:when test="contains($text, $replace)">
        <xsl:value-of select="substring-before($text,$replace)"/>
        <xsl:value-of select="$by" />
        <xsl:call-template name="string-replace-all">
          <xsl:with-param name="text"
              select="substring-after($text,$replace)"/>
          <xsl:with-param name="replace" select="$replace"/>
          <xsl:with-param name="by" select="$by"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$text" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
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
      <tr class="headerDetail" style="text-align:center;">
        <td>
          <xsl:value-of select="xslLogFormat:GetLogResource('product')" />
        </td>
        <td colspan="3">
          <xsl:value-of select="xslLogFormat:GetLogResource('BetLimit')" />
        </td>
      </tr>
      <xsl:for-each select="stakelevel[count(.|key('groupid',@groupid)[1])=1]">
        <tr class="headerSportTypeDetail">
          <td colspan="4">
            <xsl:value-of select="xslLogFormat:GetProductGroupName(@groupid)" />
          </td>
        </tr>
        <xsl:for-each select="key('groupid', @groupid)">
          <tr>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetDisplayName(@settinglevel,@sporttype,@bettype,@productid)" />
            </td>
            <td class="colW2" colspan="3">
              <xsl:call-template name="string-replace-all">
                <xsl:with-param name="text" select="@stakelevellist" />
                <xsl:with-param name="replace" select="'_'" />
                <xsl:with-param name="by" select="xslLogFormat:GetLogResource('unassigned')" />
              </xsl:call-template>
            </td>
          </tr>
        </xsl:for-each>
      </xsl:for-each>
    </table>
  </xsl:template>
  
</xsl:stylesheet>