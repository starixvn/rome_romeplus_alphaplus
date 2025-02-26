<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:output method="html" indent="no" />
  <xsl:template name="buildDetailHeader">
    <xsl:choose>
      <xsl:when test="@productid">
        <xsl:value-of select="xslLogFormat:GetProductName(@productid)" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:choose>
          <xsl:when test="@sporttype">
            <xsl:choose>
              <xsl:when test="@bettype">
                <xsl:value-of select="xslLogFormat:GetSportName(@sporttype)" /> -
                <xsl:value-of select="xslLogFormat:GetBetTypeName(@bettype)" />
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="xslLogFormat:GetSportName(@sporttype)" />
              </xsl:otherwise>
            </xsl:choose>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="xslLogFormat:GetBetTypeName(@bettype)" />
          </xsl:otherwise>
        </xsl:choose>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>
