<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />
  <xsl:template match="r">
    <table class="tblDesc">
      <tr>
        <xsl:choose>
		    <xsl:when test="@wrongpasswordalert">
			    <td class="colW1">
				    <xsl:value-of select="xslLogFormat:GetLogResource('WrongPasswordAlert')" />
			    </td>
			    <td class="colW2">
				    <xsl:value-of select="@wrongpasswordalert" />
			    </td>
		    </xsl:when>
			<xsl:when test="@systemannouncement">
				<td class="colW1">
					<xsl:value-of select="xslLogFormat:GetLogResource('SystemAnnouncement')" />
				</td>
				<td class="colW2">
					<xsl:value-of select="@systemannouncement" />
				</td>
			</xsl:when>
			<xsl:when test="@mywatchlist">
				<td class="colW1">
					<xsl:value-of select="xslLogFormat:GetLogResource('MyWatchlist')" />
				</td>
				<td class="colW2">
					<xsl:value-of select="@mywatchlist" />
				</td>
			</xsl:when>
          <xsl:when test="@abnormalloginalert">
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('AbnormalLoginAlert')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@abnormalloginalert" />
            </td>
          </xsl:when>
          <xsl:when test="@changesecuritycodealert">
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('ChangeSecurityCodeAlert')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@changesecuritycodealert" />
            </td>
          </xsl:when>
          <xsl:when test="@changepasswordalert">
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('ChangePasswordAlert')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@changepasswordalert" />
            </td>
          </xsl:when>
          <xsl:when test="@memberwinlosslimit">
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('MemberWinLossLimit')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@memberwinlosslimit" />
            </td>
          </xsl:when>
          <xsl:when test="@problemaccount">
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('ProblemAccount')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@problemaccount" />
            </td>
          </xsl:when>
          <xsl:when test="@personalmessage">
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('PersonalMessage')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@personalmessage" />
            </td>
          </xsl:when>
          <xsl:when test="@yesterdaywinloss">
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('YesterdayWinLoss')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@yesterdaywinloss" />
            </td>
          </xsl:when>
		    <xsl:when test="@otp">
			    <td class="colW1">
				    <xsl:value-of select="xslLogFormat:GetLogResource('OTP')" />
			    </td>
			    <td class="colW2">
				    <xsl:value-of select="@otp" />
			    </td>
		    </xsl:when>
          <xsl:otherwise>
            <td class="colW1"></td>
            <td class="colW2"></td>
          </xsl:otherwise>
        </xsl:choose>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('subaccname')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@subaccname" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('IP')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="xslLogFormat:FormatIP(@ip)" />
        </td>
      </tr>
    </table>
  </xsl:template>
</xsl:stylesheet>