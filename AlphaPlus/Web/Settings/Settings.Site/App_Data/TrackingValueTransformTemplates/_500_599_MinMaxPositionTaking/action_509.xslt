<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
    <xsl:param name="IsInternal" select="0"></xsl:param>
    <xsl:output method="html" indent="no" />
    <xsl:template match="r">
      <table class="tblDesc">
        <tr>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('product')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="xslLogFormat:GetProductName(@productid)" />
          </td>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('subAccName')" />
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
        <xsl:for-each select="pt">
          <tr>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('Sport')" />
              <xsl:if test="@bettype">
                - <xsl:value-of select="xslLogFormat:GetLogResource('bettype')" />
              </xsl:if>
            </td>
            <td class="colW2">
              <xsl:value-of select="xslLogFormat:GetSportName(@sporttype)" />
              <xsl:if test="@bettype">
                - <xsl:value-of select="xslLogFormat:GetBetTypeName(@bettype)" />
              </xsl:if>
            </td>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('IsLive')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@islive" />
            </td>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('lblMinPTMaster')" />
            </td>
            <td  class="colW2">
              <xsl:value-of select="@masterminpt" />
            </td>
          </tr>
        </xsl:for-each>
      </table>
    </xsl:template>
</xsl:stylesheet>