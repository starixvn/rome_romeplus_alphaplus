<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:output method="html" indent="no" />
  <xsl:include href="./headerGroup.xslt" />
  <xsl:include href="./SportsBook/directMemberCommission.xslt" />
  <xsl:include href="./SportsBook/betSetting.xslt" />
  <xsl:include href="./SportsBook/parlayMaxPayout.xslt" />
  <xsl:include href="./RNGGames/betSetting.xslt" />
  <xsl:include href="./OtherGames/betSetting.xslt" />
  <xsl:include href="./PositionTaking/directMember.xslt" />
  <xsl:include href="./memberCommission.xslt" />
  <xsl:include href="./winLossLimit.xslt" />
  <xsl:include href="./betLimit.xslt" />
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />
  <xsl:template match="root">
    <xsl:variable name="uplineroleid" select="@uplineroleid" />
    <table class="tblDesc">
      <tr>
        <td colspan="6">
          <xsl:value-of select="$uplineroleid" />
        </td>
      </tr>
      <xsl:for-each select="group">
        <xsl:if test="*/r">
          <xsl:variable name="groupid" select="@id" />
          <xsl:call-template name="headerGroup">
            <xsl:with-param name="groupid" select="$groupid" />
          </xsl:call-template>
          <tr>
            <td colspan="6">
              <xsl:variable name="style">
                <xsl:choose>
                  <xsl:when test="$groupid=1">
                    <xsl:value-of select="title" />
                  </xsl:when>
                </xsl:choose>
              </xsl:variable>
              <table class="tblDesc tbl-last">
                <xsl:if test="$groupid!=1">
                  <xsl:attribute name="style">
                    <xsl:value-of select="'display:none;'" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:apply-templates select="discount" />
                <xsl:apply-templates select="betsetting" />
                <xsl:apply-templates select="parlaymaxpayout" />
                <xsl:call-template name="directMemberPT">
                  <xsl:with-param name="groupid" select="$groupid" />
                  <xsl:with-param name="uplineroleid" select="$uplineroleid" />
                </xsl:call-template>
                <xsl:apply-templates select="winlosslimit" />
                <xsl:call-template name="betLimit" />
              </table>
            </td>
          </tr>
        </xsl:if>
      </xsl:for-each>
    </table>
  </xsl:template>
</xsl:stylesheet>