# 🌌 Zabbix Observatory for Azure

![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/Digital-Origin-Solutions/Zabbix-Observatory-for-Azure/on-push.yml?branch=master)
![GitHub Release (latest by date)](https://img.shields.io/github/v/release/Digital-Origin-Solutions/Zabbix-Observatory-for-Azure)
![GitHub License](https://img.shields.io/github/license/Digital-Origin-Solutions/Zabbix-Observatory-for-Azure)

## Overview

**Zabbix Observatory for Azure** is a focused Azure Function App that monitors critical Azure security and recovery services, providing visibility and alerts directly into Zabbix.

> ⚠️ **Important:** This project is designed **exclusively** to run as an **Azure Function App**. Running it outside of this environment is not supported or recommended.

> 🔐 **Authentication Note:** The observatory uses an **Azure Managed Identity** to securely authenticate and access the necessary Azure resources—no secrets or credentials need to be stored in the codebase.

---

## 🚀 What It Monitors

This observatory collects and reports on the following Azure resources:

- 🔐 **Application Registration Secrets**  
  Detects App Registration client secrets that are nearing expiration or already expired.

- 🤝 **Partner Relationships**  
  Monitors Azure AD B2B partner relationships for any changes or unexpected configurations.

- 💾 **Azure Recovery Services Vaults**  
  Tracks backup status, recent job results, and overall vault health to ensure business continuity.

---

## 📦 Deployment (Azure Function App)

> _Deployment instructions coming soon..._

---

## 📊 Integration with Zabbix

- The observatory pushes metrics to your Zabbix server via the appropriate endpoint.
- You can use custom Zabbix templates to:
  - Alert on secrets expiring within a configurable threshold
  - Detect new or missing partner relationships
  - Monitor failed backup jobs or degraded recovery vaults

---

## 🤝 Contributing

We welcome contributions!  
To get involved:

1. Fork this repository  
2. Create a feature or fix branch  
3. Open a Pull Request with clear descriptions

---

## 📄 License

This project is licensed under the [GNU GPLv3](LICENSE).

---

## 🧭 Support & Feedback

Have questions or issues?  
Feel free to [open an issue](https://github.com/Digital-Origin-Solutions/Zabbix-Observatory-for-Azure/issues) in the repository.
