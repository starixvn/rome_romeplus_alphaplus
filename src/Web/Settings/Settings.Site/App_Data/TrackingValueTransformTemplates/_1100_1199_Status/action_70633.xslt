<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
    <xsl:param name="IsInternal" select="0"></xsl:param>
    <xsl:output method="html" indent="no" />

    <xsl:template match="age_cashservice_custservicetypeupdate">
        <table class="tblDesc">
            <tr>
                <td class="colW1">
                <xsl:value-of select="xslLogFormat:GetLogResource('subAccName')" />
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
                <xsl:value-of select="xslLogFormat:GetLogResource('isactive')" />
                </td>
                <td class="colW2">
                <xsl:value-of select="@isactive" />
                </td>
            </tr>
        </table>
    </xsl:template>
</xsl:stylesheet>
