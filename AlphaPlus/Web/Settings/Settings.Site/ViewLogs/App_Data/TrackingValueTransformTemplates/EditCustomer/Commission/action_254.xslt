<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:output method="html" indent="no" />
  <xsl:template match="root">
    <table class="tblDesc">
      <tr>
        <td colspan="6">
          <table class="tblDesc">
            <tr>
              <xsl:apply-templates select="comm[position() &lt; 4]" />
            </tr>
            <tr>
              <xsl:apply-templates select="comm[position() &gt;= 4]" />
            </tr>
          </table>
        </td>
      </tr>
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
    </table>
  </xsl:template>
  <xsl:template match="comm">
    <td class="colW1">
      <xsl:value-of select="xslLogFormat:GetSportbookCommissionName(@sporttype,@bettype)" />
    </td>
    <td class="colW2">
      <xsl:value-of select="@playerdiscount" />
    </td>
  </xsl:template>
</xsl:stylesheet>