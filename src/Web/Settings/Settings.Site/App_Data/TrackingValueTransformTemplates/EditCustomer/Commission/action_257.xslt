<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:output method="html" indent="no" />
  <xsl:template match="root">
    <table class="tblDesc">
      <xsl:if test="comm">
        <tr>
          <td colspan="6">
            <table class="tblDesc">
              <tr class="headerSportTypeDetail" style="text-align:center;">
                <td></td>
                <td>
                  <xsl:value-of select="xslLogFormat:GetLogResource('CommForSuper')" />
                </td>
                <td>
                  <xsl:value-of select="xslLogFormat:GetLogResource('CommForMember')" />
                </td>
              </tr>
              <xsl:for-each select="comm">
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetSportbookMemberCommissionName(@sporttype,@bettype)" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@superdiscount" />
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
          <xsl:value-of select="xslLogFormat:GetLogResource('ugroup')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@ugroup" />
        </td>
      </tr>
    </table>
  </xsl:template>
</xsl:stylesheet>