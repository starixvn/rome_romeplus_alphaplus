import { useState } from "react";
import { CopyToClipboard } from "react-copy-to-clipboard";
import { useTranslation } from "react-i18next";

import { IconthumbsUp } from "@/features/Shared/Icons";
import WrapperTooltip from "@/features/Shared/WrapperTooltip";

import styles from "./NewAccountPopup.module.scss";

interface NewAccountSuccessPopupProps {
  username: string;
  credit: string;
  domain: string;
  handleOk: () => void;
}

export default function NewAccountSuccessPopup({ username, credit, domain, handleOk }: NewAccountSuccessPopupProps) {
  const { t } = useTranslation();
  const [copied, setCopied] = useState(false);
  const accountInfo = `${t("username")}: ${username}\n${t("credit")}: ${credit}\n${t("domain")}: ${domain}`;

  const handleCopyAccountInfo = () => {
    setCopied(true);
    setTimeout(() => {
      setCopied(false);
    }, 1500);
  };

  return (
    <>
      <div>
        <div className={styles.IconthumbsUp}>{IconthumbsUp}</div>
        <div className={styles.success}>{t("success")}</div>

        <div className={styles["account-info"]}>
          <div>
            <span style={{ color: "red" }}>* </span>
            <span style={{ fontStyle: "italic", opacity: 0.8 }}>{t("introCopyNote")}</span>
          </div>
          <div>
            <CopyToClipboard text={accountInfo} options={{ format: "text/plain" }}>
              <button
                type="button"
                className={styles["copy-btn"]}
                data-tooltip-id="copy-username-id"
                onClick={handleCopyAccountInfo}>
                {t("copy")}
              </button>
            </CopyToClipboard>
          </div>
          <div className={styles["info-copy"]}>
            <div className={styles["label"]}>{t("username")}</div>
            <div className={styles["row-value"]}>{username}</div>
          </div>
          <div className={styles["info-copy"]}>
            <div className={styles["label"]}>{t("credit")}</div>
            <div className={styles["row-value"]}>{credit}</div>
          </div>
          <div className={styles["info-copy"]}>
            <div className={styles["label"]}>{t("domain")}</div>
            <div className={styles["row-value"]}>{domain}</div>
          </div>
          <button className={styles.buttonSuccess} type="button" onClick={handleOk}>
            {t("okay")}
          </button>
        </div>
      </div>
      <WrapperTooltip content={t("copied") ?? ""} id="copy-username-id" type="default" isOpen={copied} />
    </>
  );
}
