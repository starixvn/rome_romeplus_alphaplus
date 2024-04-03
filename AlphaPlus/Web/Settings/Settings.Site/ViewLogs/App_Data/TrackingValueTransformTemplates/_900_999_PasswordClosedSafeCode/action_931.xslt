<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />

  <xsl:template match="age_otp_customerstatusupdafterverifiedotp">
    <table class="tblDesc">
      <tr>
		<td class="colW1">
			<xsl:value-of select="xslLogFormat:GetLogResource('SubAccount')" />
		</td>
		<td class="colW2">
			<xsl:value-of select="@subaccount" />
		</td>
		<td class="colW1">
			<xsl:value-of select="xslLogFormat:GetLogResource('checkedotp')" />
		</td>
		<td class="colW2">
			<xsl:value-of select="@checkedotp"/>
		</td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('ip')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="xslLogFormat:FormatIP(@ip)" />
        </td>
      </tr>
	    <tr>
		    <td class="colW1">
			    <xsl:value-of select="xslLogFormat:GetLogResource('language')" />
		    </td>
		    <td class="colW2">
			    <xsl:value-of select="xslLogFormat:FormatIP(@language)" />
		    </td>
			<td class="colW1"></td>
			<td class="colW2"></td>
			<td class="colW1"></td>
			<td class="colW2"></td>
	    </tr>
    </table>
  </xsl:template>
</xsl:stylesheet>
