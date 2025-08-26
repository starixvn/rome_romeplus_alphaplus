<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />
  <xsl:template match="root">
    <xsl:choose>
      <xsl:when test="@custid">
        <table class="tblDesc">
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('SubAccName')" /></td>
            <td class="colW2"><xsl:value-of select="@subaccname" /></td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('IP')" /></td>
            <td class="colW2"><xsl:value-of select="xslLogFormat:FormatIP(@ip)" /></td>
          </tr>
        </table>
        <xsl:for-each select="promotion">
          <table class="tblDesc">
            <tr>
              <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('Type')" /></td>
              <td class="colW2"><xsl:value-of select="xslLogFormat:GetTrackingPromotionTypeName(@type)" /></td>
              <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('Status')" /></td>
              <td class="colW2"><xsl:value-of select="xslLogFormat:GetTrackingStatus(@enabled)" /></td>
              <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('From')" /></td>
              <td class="colW2"><xsl:value-of select="@fromdate" /></td>
              <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('To')" /></td>
              <td class="colW2" colspan="3"><xsl:value-of select="@todate" /></td>
            </tr>
            <tr class="headerDetail" onclick="toggle(this)">
              <td class="colW1" colspan="10"><xsl:value-of select="xslLogFormat:GetLogResource('Details')" /></td>
            </tr>
            <xsl:for-each select="details">
              <xsl:for-each select="r">
                <tr>
                  <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('ProductCategory')" /></td>
                  <td class="colW2"><xsl:value-of select="@productcategoryid" /></td>
                  <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('product')" /></td>
                  <td class="colW2"><xsl:value-of select="xslLogFormat:GetProductNames(@product)" /></td>
                  <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('Percentage')" /></td>
                  <td class="colW2"><xsl:value-of select="@percentage" /></td>
                  <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('MinTO')" /></td>
                  <td class="colW2"><xsl:value-of select="@minto" /></td>
                  <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('MaxPayout')" /></td>
                  <td class="colW2"><xsl:value-of select="@maxpayout" /></td>
                </tr>
              </xsl:for-each>
            </xsl:for-each>
            <tr class="headerDetail" onclick="toggle(this)">
              <td class="colW1" colspan="10"><xsl:value-of select="xslLogFormat:GetLogResource('Content')" /></td>
            </tr>
            <xsl:for-each select="contents">
              <xsl:for-each select="r">
                <tr>
                  <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('language')" /></td>
                  <td class="colW2"><xsl:value-of select="@language" /></td>
                  <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('Title')" /></td>
                  <td class="colW2"><xsl:value-of select="@title" /></td>
                  <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('Content')" /></td>
                  <td class="colW2" colspan="5"><div class="valuecontent"><xsl:value-of select="xslLogFormat:HtmlDecode(@content)" /></div></td>
                </tr>
              </xsl:for-each>
            </xsl:for-each>
          </table>
        </xsl:for-each>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>
