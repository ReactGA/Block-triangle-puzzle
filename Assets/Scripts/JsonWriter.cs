using System.IO;
using System.Collections.Generic;
using UnityEngine;
using MiniJSONV;

public class JsonWriter : MonoBehaviour
{
    [SerializeField] string path, name;
    [SerializeField] int levelCount = 5;//Multiple of 5s
    void Start()
    {
        if (levelCount % 5 != 0)
        {
            Debug.LogError("LevelCount not multiple of 5");
            return;
        }
        ConfigCalc();
        path += $"/{name}.json";
        var jsondata = Json.Serialize(data2());
        File.WriteAllText(path, jsondata);
        Debug.Log("data Put!");
    }
    int lvlSect()
    {
        return levelCount / 5;
    }
    Dictionary<string, object> data1()
    {
        return new Dictionary<string, object>() { { "levels", TotalConfig(levelConfig()) } };
    }
    Dictionary<string, object> data2()
    {
        return new Dictionary<string, object>() { { "levels", HintConfig() } };
    }
    Dictionary<string, object> HintConfig()
    {
        //List<object> config = new List<object>();
        Dictionary<string, object> config = new Dictionary<string, object>();

        for (int i = 0; i < levelCount; i++)
        {
            var dict = new Dictionary<string, object>(){
            //{"levelNo",i+1},
            {"RelativeStartPos",new Dictionary<string,int>(){
                {"x",540},
                {"y",766},
                {"z",0},
            }},
            {"hint1",new Dictionary<string,int>(){
                {"x1",0},
                {"y1",0},
                {"x2",0},
                {"y2",0},
                {"x3",0},
                {"y3",0},
                {"x4",0},
                {"y4",0}
            }},
            {"hint2",new Dictionary<string,int>(){
                {"x1",0},
                {"y1",0},
                {"x2",0},
                {"y2",0},
                {"x3",0},
                {"y3",0},
                {"x4",0},
                {"y4",0}
            }},
            {"Color",1},
        };
            config.Add((i+1).ToString(),dict);
        }
        return config;
    }
    List<object> shapeConfig()
    {
        return new List<object>(){
                    new Dictionary<string,object>(){
                        {"id","34567890"},
                        {"pieces",new List<Dictionary<string,object>>(){
                            new Dictionary<string, object>(){
                                { "coordinate",new Dictionary<string, int>(){
                                    {"x", 0},
                                    {"y", 0}
                                }
                            }},

                            new Dictionary<string, object>(){
                                { "coordinate",new Dictionary<string, int>(){
                                    {"x", 0},
                                    {"y", 0}
                                }
                            }},
                            new Dictionary<string, object>(){
                                { "coordinate",new Dictionary<string, int>(){
                                    {"x", 0},
                                    {"y", 0}
                                }
                            }},
                        }},
                        {"color",Random.Range(1,7)},
                        {"coordinate",new Dictionary<string,object>(){
                            {"x",0},
                            {"y",0}
                        }},
                        {"baseInverted",false},
                        },
                    new Dictionary<string,object>(){
                        {"id","34567890"},
                        {"pieces",new List<Dictionary<string,object>>(){
                            new Dictionary<string, object>(){
                                { "coordinate",new Dictionary<string, int>(){
                                    {"x", 0},
                                    {"y", 0}
                                }
                            }},

                            new Dictionary<string, object>(){
                                { "coordinate",new Dictionary<string, int>(){
                                    {"x", 0},
                                    {"y", 0}
                                }
                            }},
                            new Dictionary<string, object>(){
                                { "coordinate",new Dictionary<string, int>(){
                                    {"x", 0},
                                    {"y", 0}
                                }
                            }},
                        }},
                        {"color",Random.Range(1,7)},
                        {"coordinate",new Dictionary<string,object>(){
                            {"x",0},
                            {"y",0}
                        }},
                        {"baseInverted",false},
                        },
                    new Dictionary<string,object>(){
                        {"id","34567890"},
                        {"pieces",new List<Dictionary<string,object>>(){
                            new Dictionary<string, object>(){
                                { "coordinate",new Dictionary<string, int>(){
                                    {"x", 0},
                                    {"y", 0}
                                }
                            }},

                            new Dictionary<string, object>(){
                                { "coordinate",new Dictionary<string, int>(){
                                    {"x", 0},
                                    {"y", 0}
                                }
                            }},
                            new Dictionary<string, object>(){
                                { "coordinate",new Dictionary<string, int>(){
                                    {"x", 0},
                                    {"y", 0}
                                }
                            }},
                        }},
                        {"color",Random.Range(1,7)},
                        {"coordinate",new Dictionary<string,object>(){
                            {"x",0},
                            {"y",0}
                        }},
                        {"baseInverted",false},
                        }
            };
    }

    List<object> levelConfig()
    {
        List<object> configList = new List<object>();
        for (int i = 0; i < levelCount; i++)
        {
            List<Dictionary<string, int>> list = new List<Dictionary<string, int>>();
            var tilesCount = TileCountPerLevel(i + 1);//algorithm
            for (int j = 0; j < tilesCount; j++)
            {
                Dictionary<string, int> dict = new Dictionary<string, int>();
                var pt = levelPoint(j, i + 1);
                dict.Add("x", pt.x);//algorithm
                dict.Add("y", pt.y);//algorithm
                list.Add(dict);
            }

            configList.Add(list);
        }
        return configList;
    }
    int[] numPerRowX, numPerRowY;
    void ConfigCalc()
    {
        numPerRowX = new int[levelCount];
        numPerRowY = new int[levelCount];
        for (int i = 0; i < levelCount; i++)
        {
            numPerRowX[i] = i <= lvlSect() * 1 ? Random.Range(4, 6) :
                (i <= lvlSect() * 3 && i > lvlSect() * 1) ? Random.Range(5, 7) :
                (i <= lvlSect() * 5 && i > lvlSect() * 3) ? Random.Range(6, 8) : 0;

            numPerRowY[i] = i <= lvlSect() * 1 ? Random.Range(2, 4) :
                (i <= lvlSect() * 3 && i > lvlSect() * 1) ? Random.Range(3, 5) :
                (i <= lvlSect() * 5 && i > lvlSect() * 3) ? Random.Range(4, 7) : 0;
        }
    }
    int BeginXPos(int i)
    {
        return numPerRowX[i] % 2 == 0 ? -(numPerRowX[i] / 2) : -((numPerRowX[i] / 2) + Random.Range(0, 2));
    }
    int BeginYPos(int i)
    {
        var p = numPerRowY[i];
        return p == 2 ? 2 : p == 3 ? 2 : p == 4 ? 1 : p == 5 ? 1 : p == 6 ? 1 : -100;
    }
    int lastXPos, lastYPos, lastlevel;
    bool started;
    int Xpos(int i, int lvl)
    {
        if (lvl > lastlevel)
        {
            lastlevel = lvl;
            //Debug.Log("Begin: "+ BeginYPos(lvl-1));
            lastYPos = BeginYPos(lvl - 1);
            lastXPos = BeginXPos(lvl - 1) - 1;
        }

        var val = Random.value;
        int offset = val < 0.005 ? 2 : val < 0.01 ? 1 : 0;
        if (lastXPos >= Mathf.Abs(BeginXPos(lvl - 1)))
        {
            lastXPos = BeginXPos(lvl - 1) + offset;
            lastYPos++;
        }
        else
        {
            lastXPos += (offset + 1);
        }
        return lastXPos;
    }
    Vector2Int levelPoint(int tileIndex, int level)
    {
        var pos = new Vector2Int(Xpos(tileIndex, level), lastYPos);
        //Debug.Log(pos +" tile: "+ tileIndex + " level: " + level);
        return pos;
    }
    int TileCountPerLevel(int level)
    {
        var i = level;
        return i <= lvlSect() * 1 ? Random.Range(10, 15) :
        (i <= lvlSect() * 3 && i > lvlSect() * 1) ? Random.Range(15, 20) :
        (i <= lvlSect() * 5 && i > lvlSect() * 3) ? Random.Range(20, 25) : 0;
    }
    List<object> TotalConfig(List<object> levelConfig, int levelIndex = 0)
    {
        List<object> ConfigData = new List<object>();
        for (int i = 0; i < levelCount; i++)
        {
            Dictionary<string, object> levelDict = new Dictionary<string, object>();
            levelDict.Add("shapeDatas", shapeConfig());
            levelDict.Add("levelNo", i + 1);
            levelDict.Add("boardData", new Dictionary<string, object>(){
                {"tiles",levelConfig[i]}
            });

            ConfigData.Add(levelDict);
        }
        return ConfigData;
    }
    Dictionary<string, object> data()
    {
        var obj = new List<object>(){
            
            //first level
            new Dictionary<string,object>(){
                {"shapeDatas", new List<object>(){
                    new Dictionary<string,object>(){
                        {"id","34567890"},
                        {"pieces",new List<Dictionary<string,object>>(){ //summarize into a class
                            new Dictionary<string, object>(){
                                { "coordinate",new Dictionary<string, int>(){
                                    {"x", 0},
                                    {"y", 0}
                                }
                            }},

                            new Dictionary<string, object>(){
                                { "coordinate",new Dictionary<string, int>(){
                                    {"x", 0},
                                    {"y", 0}
                                }
                            }},
                            new Dictionary<string, object>(){
                                { "coordinate",new Dictionary<string, int>(){
                                    {"x", 0},
                                    {"y", 0}
                                }
                            }},
                        }},
                        {"color",6},
                        {"coordinate",new Dictionary<string,object>(){
                            {"x",0},
                            {"y",0}
                        }},
                        {"baseInverted",false},

                    },

                    //second tile piece
                    new Dictionary<string,object>(){
                        {"id","34567890"},
                        {"pieces",new List<Dictionary<string,object>>(){ //summarize into a class
                            new Dictionary<string, object>(){
                                { "coordinate",new Dictionary<string, int>(){
                                    {"x", 0},
                                    {"y", 0}
                                }
                            }},

                            new Dictionary<string, object>(){
                                { "coordinate",new Dictionary<string, int>(){
                                    {"x", 0},
                                    {"y", 0}
                                }
                            }},
                        }},
                        {"color",6},
                        {"coordinate",new Dictionary<string,object>(){
                            {"x",0},
                            {"y",0}
                        }},
                        {"baseInverted",false},

                    },
                }},
                {"levelNo",1},
                {"boardData",new Dictionary<string,object>(){
                    {"tiles",new List<Dictionary<string,int>>(){ //summarize into a class
                        new Dictionary<string, int>(){
                            {"x", 0},
                            {"y", 0}
                        },
                        new Dictionary<string, int>(){
                            {"x", 1},
                            {"y", 0}
                        }
                    }}
                }},
            },

            //Second level
            new Dictionary<string,object>(){
                {"shapeDatas", new List<object>(){
                    new Dictionary<string,object>(){
                        {"id","34567890"},
                        {"pieces",new List<Dictionary<string,object>>(){ //summarize into a class
                            new Dictionary<string, object>(){
                                { "coordinate",new Dictionary<string, int>(){
                                    {"x", 0},
                                    {"y", 0}
                                }
                            }},

                            new Dictionary<string, object>(){
                                { "coordinate",new Dictionary<string, int>(){
                                    {"x", 0},
                                    {"y", 0}
                                }
                            }},
                        }},
                        {"color",6},
                        {"coordinate",new Dictionary<string,object>(){
                            {"x",0},
                            {"y",0}
                        }},
                        {"baseInverted",false},

                    },

                    //second tile piece
                    new Dictionary<string,object>(){
                        {"id","34567890"},
                        {"pieces",new List<Dictionary<string,object>>(){ //summarize into a class
                            new Dictionary<string, object>(){
                                { "coordinate",new Dictionary<string, int>(){
                                    {"x", 0},
                                    {"y", 0}
                                }
                            }},

                            new Dictionary<string, object>(){
                                { "coordinate",new Dictionary<string, int>(){
                                    {"x", 0},
                                    {"y", 0}
                                }
                            }},
                        }},
                        {"color",6},
                        {"coordinate",new Dictionary<string,object>(){
                            {"x",0},
                            {"y",0}
                        }},
                        {"baseInverted",false},

                    },
                }},
                {"levelNo",1},
                {"boardData",new Dictionary<string,object>(){
                    {"tiles",new List<Dictionary<string,int>>(){ //summarize into a class
                        new Dictionary<string, int>(){
                            {"x", 0},
                            {"y", 0}
                        },
                        new Dictionary<string, int>(){
                            {"x", 1},
                            {"y", 0}
                        }
                    }}
                }},
            },

            ////third level new Ditionary...
        };
        return new Dictionary<string, object>() { { "levels", obj } };
    }
}