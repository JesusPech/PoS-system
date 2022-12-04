# PoS-system
POS terminal project returns optimal change.
Solution has two projects ApplicationCore and PoS_TestProject. ApplicationCore contains class for process and PoS_TestProject is the unit testing project.
System asks only once for Region/Country configuration. When users entered then system save configuration file (appsettings.json) on Envivorment.CurrentDirectory.

Steps for use system
1. Set region 
2. Set price item
3. Set amount of bills and coins for pay item
4. Get optimal change
