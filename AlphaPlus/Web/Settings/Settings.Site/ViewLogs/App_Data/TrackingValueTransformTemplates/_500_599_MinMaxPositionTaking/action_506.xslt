<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
    <xsl:param name="IsInternal" select="0"></xsl:param>
    <xsl:output method="html" indent="no" />
    <xsl:template match="acc_custoverrideupdkeno">
      <xsl:if test="@sporttype = 202">
      <div class="headerSportTypeDetail">
        <xsl:choose>
          <xsl:when test="@sporttype">
            <xsl:value-of select="xslLogFormat:GetSportName(@sporttype)" /> -
            <xsl:value-of select="xslLogFormat:GetBetTypeName(@bettype)" />
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="xslLogFormat:GetSportName(202)" />
          </xsl:otherwise>
        </xsl:choose>
      </div>
      </xsl:if>
		<table class="tblDesc">
			<tr>
				<td class="colW1">
					<xsl:value-of select="xslLogFormat:GetLogResource('minPT')" />
				</td>
				<td class="colW2">
					<xsl:value-of select="@min_positiontaking" />
				</td>
				<td class="colW1">
					<xsl:value-of select="xslLogFormat:GetLogResource('Language')" />
				</td>
				<td class="colW2">
					<xsl:value-of select="@language" />
				</td>
				<td class="colW1">
					<xsl:value-of select="xslLogFormat:GetLogResource('IP')" />
				</td>
				<td class="colW2">
					<xsl:value-of select="xslLogFormat:FormatIP(@ip)" />
				</td>
			</tr>
			<tr>
				<xsl:if test="@userid != 8">
					<td class="colW1">
						<xsl:value-of select="xslLogFormat:GetLogResource('SubAccName')" />
					</td>
					<td class="colW2">
						<xsl:value-of select="@subaccname" />
					</td>
				</xsl:if>
				<td class="colW1">
					<xsl:value-of select="xslLogFormat:GetLogResource('ErrCode')" />
				</td>
				<td class="colW2">
					<xsl:value-of select="xslLogFormat:FormatErrCode(@errcode)" />
				</td>
				<td class="colW1">
					<xsl:value-of select="xslLogFormat:GetLogResource('ErrMsg')" />
				</td>
				<xsl:if test="@userid = 8">
				<td class="colW2">
					<xsl:value-of select="xslLogFormat:FormatErrMsg(@errmsg)" />
				</td>
          <td class="colW1"></td><td class="colW2"></td>
				</xsl:if>
				<xsl:if test="@userid != 8">
					<td class="colW2">
						<xsl:value-of select="xslLogFormat:FormatErrMsg(@errmsg)" />
					</td>
				</xsl:if>
				
			</tr>
		</table>
    </xsl:template>

</xsl:stylesheet>
