# Mini_Metro：我的地铁
这是一款用Unity2021开发的，模仿 《模拟地铁》游戏的独立游戏。

作者：朱正阳（TsingPig）

# 开发日志

## v0.1.0 版本

素材导入

1. CityNodeTexture绘制组件 **DrawPolygon.py**

<img src="https://github.com/user-attachments/assets/c0a75fd7-f9fa-4089-892f-17944a8c0c64" alt="image-20240731221151103" width="50%">

2. 创建瓦片贴图（弃用）

<img src="https://github.com/user-attachments/assets/3a97a0ae-36d1-41ca-b35b-3b0d5ea13161" alt="image-20240801123439127" width="50%">

3. Ripple效果 （城市节点点击产生涟漪）

<img src="https://github.com/user-attachments/assets/e98007a2-8f57-4037-96ba-4ea3c8f4fdcb" alt="image-20240801154103706" width="50%">

4. **LineDrawer.cs** 地铁线条绘制效果

<img src="https://github.com/user-attachments/assets/d75fea43-9bd0-462b-ba96-87c4cdbecce1" alt="image-20240801204633601" width="50%">

## v0.2.0 版本

1. 导入MVP框架，改用AA加载资源

2. 地铁线路生成管理器 **MetroLineManager.cs**

    拖动连线、磁吸效果、创建保护。

<img src="https://github.com/user-attachments/assets/049b18ef-f317-40bc-80d8-899f851e4cb8" alt="image-20240803203858611" width="50%">

3. 车站数字标号显示

![image](https://github.com/user-attachments/assets/6a70ff8c-cdf5-43a0-bf0a-fd8f6b302484)




## v0.3.0 版本

1. 地铁列车（**MetroTrain**）
![image](https://github.com/user-attachments/assets/3aab13ad-25ea-4e8a-9034-ddc5621cbde3)

2. 
# 策划

用几何节点模拟城市区域车站（简称“车站”），市民有若干**出行需求**。

玩家的任务是通过**有限的地铁资源**，完成下面的若干目标：

- **生存模式**：尽可能让所有的地铁站**不拥堵**，运行最多的天数（不会有很多乘客在排队）

- **创意模式**：尽可能**最大化单位时间的运载客流人次**。
  
  
    $$
    \mu= \frac {所有站点在T时间内的入站人数+所有站点在T时间内的出站人数}{2}
    $$
    

## 有限的地铁资源

1. 地铁线路（MetroLine）：3 ~ 20条
2. 地铁列车（**MetroTrain**）：载客6人的动力列车头。
3. 地铁车厢（MetroCarriage）：载客6人的、需挂载至列车头的车厢节。
4. 隧道 / 桥梁（MetroBrigde）：用于穿越河流。
5. 换乘枢纽（MetroTransferHub）：提高车站的最大客容量（生存模式）、换乘效率（创意模式）。

## 车站

不同的几何图形，代表不同的城市功能区车站。例如，圆形代表居民区车站，三角形代表大型商业区车站，正方形代表学校/政府机构/写字楼等区域车站，菱形代表工业区。

| 车站类型                       | 几何图形 |
| ------------------------------ | -------- |
| 居民区车站                     | 圆形     |
| 大型商业区车站                 | 三角形   |
| 学校/政府机构/写字楼等区域车站 | 正方形   |
| 工业区车站                     | 菱形     |

## 出行需求

在车站的旁边，会出现若干实心黑色小图形。例如，一个圆形车站出现了一个三角形图形，代表这个市民有一个**前往城市中任意一个三角形车站的出行需求**。因此，合理的地铁规划，需要在能够**可达**（任一个车站的任一个需求都能满足）的情况下，尽可能减少车站拥堵，或者提高换乘效率。

车站出现的出行需求，在生存模式中，不会消失。这意味着不能满足的出行需求，会持续造成车站拥堵。在创意模式中，在等车过久后，该需求可能会消失。

## 生存模式

每个车站有一个最大客容量，如20人。过多的在该车站等候的市民，会造成车站拥堵。

## 创意模式

需要合理规划换乘线路，使得地铁运行效率最高。

## 城市发展

由于城市在不断发展，会增加新的车站需求。这意味车站会在一定时间内动态增加。场景中随机生成节点（CityNode，保证生成合理性），在生存模式和创意模式中，玩家允许重新设置线路。
