# GameFramework


GFrame

------Scripts

      ------Engine
      
            ------DataRecord   存档
            
                  #DataClassHandler
                        存档的核心类
                        
                  #DataDirtyHandler
                        
                  #IDataClass
                        继承了DataDirtyHandler，提供了InitWithEmptyData()和OnDataLoadFinish()方法
            ------ResSystem 资源系统
                  ------AssetDataTable
                        #ABUnit     
                              包含一个AB的如下信息 name;Depends;MD5;FileSize;BuildTime;
                        #AssetData
                              AssetName;ResType;Index;
                        #AssetDataPackage
                              AssetDatabase.GetAssetPathsFromAssetBundle(AB包名);
                              获取一个AB包内的所有资源，将其保存成多个AssetData
                              
                       
                        #AssetDataTable  
                              AssetDatabase.GetAllAssetBundleNames()
                              获取所有的AB包，将其保存成多个AssetDataPackage
                              func:
                              AddAssetBundle
                       
                  ------Core
                        ------Res
                        ------ResLoader
                        #ResMgr
                              
                  ------Editor
                        -----—-AssetBundle
                               AssetAutoProcess 
                               继承AssetPostprocessor 用于当资源导入或移动的时候自动分配ABName
                   

