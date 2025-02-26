<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:output method="html" indent="no" />
  <xsl:template name="headerGroup">
    <xsl:param name="groupid" />
    <tr class="headerGroup" onclick="toggle(this)">
      <xsl:attribute name="class">
        <xsl:choose>
          <xsl:when test="$groupid=1">
            <xsl:value-of select="string('headerGroup ')" />
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="string('headerGroup toggle-icon')" />
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <td colspan="6">
        <span class="icon-chevron-down"></span>
        <xsl:value-of select="xslLogFormat:GetProductGroupName(@id)" />
      </td>
    </tr>
  </xsl:template>
</xsl:stylesheet>