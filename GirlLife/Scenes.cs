//using Engine;
//using Engine.Events;
//using Engine.UI;

//namespace GirlLife {
//    public static class Scenes {
//        public static void CreateScenes() {
//            new Scene(
//                new Act(
//                    "Scene B",
//                    new EngineEvent() {
//                        Action = UI.Actions.Notify("This should be after 'went to'"),
//                        Priority = 2,
//                        TimesToInvokeAction = 2
//                    },
//                    new EngineEvent() {
//                        Action = UI.Actions.Notify("This should be before 'went to'"),
//                        Priority = -1,
//                        TimesToInvokeAction = 3
//                    },
//                    new EngineEvent() {
//                        FireCondition = Game.FireConditions.HasFlag("WentToSceneC"),
//                        Action = UI.Actions.Notify("Scene C was executed before")
//                    },
//                    new EngineEvent() {
//                        Action = () => Game.Counters.Add("ABTimes", 0),
//                        Priority = -1
//                    },
//                    new EngineEvent() {
//                        Action = () => UI.Actions.Notify($"Went to SceneB from SceneA {++Game.Counters["ABTimes"]} times"),
//                        DestructionCondition = EngineEvent.DestructionConditions.Never()
//                    }
//                ) {
//                    Text = "To SceneB"
//                },
//                new Act(
//                    "Scene C",
//                    new EngineEvent() {
//                        Action = Game.Actions.SetFlag("WentToSceneC")
//                    }
//                ) {
//                    Text = "To Scene C"
//                },
//                new Act("Chained Scene") { Text = "To Chained Scene" }
//            ) {
//                Name = "Scene A",
//                Body = "Something's happening in Scene A"
//            }.Register("Scene A");

//            new Scene(
//                new Act(
//                    "Scene A"
//                ) {
//                    Text = "To Scene A"
//                },
//                new Act(
//                    new EngineEvent() {
//                        Action = () => {
//                            Scene.GetScene("Scene A").OnStart.Add(new EngineEvent() {
//                                Action = UI.Actions.Notify("Set in Scene B for Scene A"),
//                                DestructionCondition = EngineEvent.DestructionConditions.NTimes(3)
//                            });
//                        },
//                        DestructionCondition = EngineEvent.DestructionConditions.Never()
//                    }
//                ) {
//                    Text = "Activate Scene A event"
//                }
//            ) {
//                Name = "Scene B",
//                Body = "Something's happening in Scene B"
//            }.Register("Scene B");

//            new Scene(
//                new Act(
//                    new EngineEvent() {
//                        Action = State.Actions.StopRunning()
//                    }
//                ) {
//                    Text = "Stop"
//                },
//                new Act("Scene A") {
//                    Text = "To Scene A"
//                }
//            ) {
//                Name = "Scene C",
//                Body = "Something's happening in Scene C"
//            }.Register("Scene C");

//            Scene.Chain(
//                new Scene() { Body = "First Scene" },
//                new Scene() { Body = "Second Scene" },
//                new Scene(new Act("Scene A") { Text = "To Scene A" }) { Body = "Third Scene" }
//            ).Register("Chained Scene");

//            foreach (var scene in Scene.AllScenes) {
//                foreach (var act in scene.Acts) {
//                    act.OnUse.Add(new EngineEvent() {
//                        FireCondition = act.HasNextScene,
//                        Action = () => UI.Actions.Notify($"Went from {scene.Name} to {act.NextScene!.Name}")(),
//                        Priority = 1,
//                        DestructionCondition = EngineEvent.DestructionConditions.Never(),
//                    });
//                }
//            }
//        }
//    }
//}
