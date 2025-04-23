<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
    <xsl:param name="IsInternal" select="0"></xsl:param>
    <xsl:output method="html" indent="no" />
    <xsl:template match="acc_customerupdsupervs_positiontaking">
		<div class="headerSportTypeDetail">
			<xsl:choose>
				<xsl:when test="@sporttype">
					<xsl:value-of select="xslLogFormat:GetSportName(@sporttype)" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="xslLogFormat:GetSportName(180)" />
				</xsl:otherwise>
			</xsl:choose>
		</div>
		<table class="tblDesc">
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('lblMaxPTSuper')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@s4" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('lblMaxPTMaster')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@m4" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('lblMaxPTAgent')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@a4" />
        </td>
      </tr>
      
			<tr>
				<td class="colW1">
					<xsl:value-of select="xslLogFormat:GetLogResource('lblMinPTSuper')" />
				</td>
				<td class="colW2">
					<xsl:value-of select="@n4" />
				</td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('lblUpdatePT')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@updatepositiontaking" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('UpdMinPos')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@updateminpositiontaking" />
        </td>
			</tr>
			<tr>

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
				<td class="colW1">
					<xsl:value-of select="xslLogFormat:GetLogResource('ErrCode')" />
				</td>
				<td class="colW2">
					<xsl:value-of select="xslLogFormat:FormatErrCode(@errcode)" />
				</td>
				
			</tr>
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('ErrMsg')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="xslLogFormat:FormatErrMsg(@errmsg)" />
        </td>
        <td class="colW1"></td><td class="colW2"></td>
        <td class="colW1"></td><td class="colW2"></td>
      </tr>
		</table>
    </xsl:template>

</xsl:stylesheet>
