<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />
  <xsl:key name="groupid" match="pt" use="@groupid" />
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
        <td class="colW1">
        </td>
        <td class="colW2">
        </td>
      </tr>

      <xsl:if test="pt">
        <tr style="text-align:center;" class="headerDetail">
          <td>
            <xsl:value-of select="xslLogFormat:GetLogResource('product')" />
          </td>
          <td>
            <xsl:value-of select="xslLogFormat:GetLogResource('lblMinPTAgent')" />
          </td>
          <td>
          </td>
          <td>
          </td>
          <td>
          </td>
          <td>
          </td>
        </tr>
        <xsl:for-each select="pt[count(.|key('groupid',@groupid)[1])=1]">
          <tr class="headerSportTypeDetail">
            <td colspan="6">
              <xsl:value-of select="xslLogFormat:GetProductGroupName(@groupid)" />
            </td>
          </tr>
          <xsl:for-each select="key('groupid', @groupid)">
            <tr>
              <td class="colW1">
                <xsl:value-of select="xslLogFormat:GetDisplayName(@settinglevel,@sporttype,@bettype,@productid)" />
              </td>
              <td class="colW2">
                <xsl:value-of select="@agentminpt" />
              </td>
              <td class="colW2">
              </td>
              <td class="colW2">
              </td>
              <td class="colW2">
              </td>
              <td class="colW2">
              </td>
            </tr>
          </xsl:for-each>
        </xsl:for-each>
      </xsl:if>
    </table>
  </xsl:template>
</xsl:stylesheet>