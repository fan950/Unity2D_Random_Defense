using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillManager
{
    public SkillData Get_SkillData(string sName);
    public void Create_Skill(int nIndex, Vector2 vecPos);
}
public class SkillManager : Singleton<SkillManager>, ISkillManager
{
    private SkillDataTable skillDataTable;
    private Dictionary<int, ObjcetPool<Skill>> dicSkill = new Dictionary<int, ObjcetPool<Skill>>();
    private List<Skill> liveActiveSkill = new List<Skill>();

    private const string sTablePath = "Table/SkillTable";

    public override void Awake()
    {
        base.Awake();

        skillDataTable = Resources.Load(sTablePath) as SkillDataTable;
        skillDataTable.Init();

        for (int i = 0; i < skillDataTable.lisSkillData.Count; ++i)
        {
            dicSkill.Add(skillDataTable.lisSkillData[i].nIndex, new ObjcetPool<Skill>());
            dicSkill[skillDataTable.lisSkillData[i].nIndex].Init(skillDataTable.lisSkillData[i].obj, 3, transform);
        }
    }

    public void Create_Skill(int nIndex, Vector2 vecPos)
    {
        Skill _skill = dicSkill[nIndex].Get();
        SkillData _skillData = skillDataTable.Get_Data(nIndex);
        _skill.Set_Skill(vecPos);
        switch (_skillData.sName)
        {
            case "Meteo":
                _skill.transform.position = new Vector3(7, 7, 0);
                break;
            case "Star":
                _skill.transform.position = new Vector3(vecPos.x, 7, 0);
                break;
        }

        _skill.Init(_skillData, CombatManager.Instance);
        liveActiveSkill.Add(_skill);
    }
    public SkillData Get_SkillData(string sName)
    {
        return skillDataTable.Get_Data(sName);
    }
    public void Update_Mgr()
    {
        if (liveActiveSkill.Count > 0)
        {
            for (int i = liveActiveSkill.Count - 1; i >= 0; --i)
            {
                if (liveActiveSkill[i].bDie)
                {
                    dicSkill[liveActiveSkill[i].nIndex].Return(liveActiveSkill[i]);
                    liveActiveSkill.RemoveAt(i);
                    continue;
                }
                liveActiveSkill[i].Update_Skill();
            }
        }
    }
}
