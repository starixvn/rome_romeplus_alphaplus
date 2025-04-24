<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
    <xsl:param name="IsInternal" select="1"></xsl:param>
    <xsl:output method="html" indent="no" />

    <xsl:template match="acc_customermaxnewmemperday_sag">
        <table class="tblDesc">
            <tr>
                <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('custid')" />
                </td>
                <td class="colW2">
                    <xsl:value-of select="@custid" />
                </td>
                <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('newvalue')" />
                </td>
                <td class="colW2">
                    <xsl:value-of select="xslLogFormat:FormatUnlimited(@newvalue)" />
                </td>
                <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('oldvalue')" />
                </td>
                <td class="colW2">
                    <xsl:value-of select="xslLogFormat:FormatUnlimited(@oldvalue)" />
                </td>
            </tr>
            <tr>
                <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('ip')" />
                </td>
                <td class="colW2">
                    <xsl:value-of select="xslLogFormat:FormatIP(@ip)" />
                </td>
              <td class="colW1"></td><td class="colW2"></td>
              <td class="colW1"></td><td class="colW2"></td>
            </tr>
        </table>
    </xsl:template>
</xsl:stylesheet>