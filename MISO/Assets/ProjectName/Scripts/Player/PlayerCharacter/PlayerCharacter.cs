﻿using UnityEngine;
using UnityEngine.Assertions;
using System;
using Ribbons;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;

/// <summary>
/// プレイヤーキャラクターオブジェクトの動作を実装するクラス。
/// </summary>
[RequireComponent(
    //typeof(Rigidbody),
    typeof(Animator),
    typeof(Movable))]
public class PlayerCharacter : MonoBehaviour
{
    public enum MainState
    {
        Start = 0,
        Movable,
        Throw,
        Hold,
        CaughtRibbon,
        CaughtHold,
        InBuilding,
    }

    public enum MovableState
    {
        Wait = 0,
        Move,
        Exit
    }

    public enum ThrowState
    {
        SizeAdjust = 0,
        LengthAdjust,
        Pull,
        Collect,
    }

    public int GetPlayerNumber()
    {
        int playerNumber = 0;

        switch (gameObject.tag)
        {
            case "Player1":
                {
                    playerNumber = 1;
                }
                break;

            case "Player2":
                {
                    playerNumber = 2;

                }
                break;

            case "Player3":
                {
                    playerNumber = 3;

                }
                break;

            case "Player4":
                {
                    playerNumber = 4;

                }
                break;

            default:
                {
                    Debug.LogAssertion("タグが設定されていません");
                    break;
                }
        }       // end of switch(gameObject.tag)

        return playerNumber;
    }

    public void InputStick(float horizontal, float vertical)
    {
        if (horizontal != .0f || vertical != .0f)
        {
            m_animatorParameters.isDownStick = true;

            Vector3 direction = new Vector3(horizontal, .0f, vertical);

            m_movable.direction = direction;

            if (m_movable.enabled ||
                m_animatorStateInfo.fullPathHash == Animator.StringToHash("Base Layer.Throw.SizeAdjust"))
            {
                transform.forward = direction;

                //m_rigidbody.mass = 0.1f;
            }

            if (m_controlledRibbon)
            {
                m_controlledRibbon.Shake(horizontal);
            }

            if (m_caughtRibbon)
            {
                m_caughtRibbon.ViolentMove(direction);

                m_bgmManager.PlaySELoop("se008_RageGirl");
            }
        }
        else
        {
            m_animatorParameters.isDownStick = false;

            m_movable.direction = Vector3.zero;

            if(m_controlledRibbon != null)
            {
                m_controlledRibbon.StickUp();
            }
        }
    }

    public void InputDash(bool isDash)
    {
        m_beforeIsDash = m_isDash;
        m_isDash = false;

        float downSpeed = 0.0f;

        if (m_animatorStateInfo.shortNameHash == Animator.StringToHash("Move"))
        {
            if(m_player.score > m_playerCharacterData.speedDownStartScore && m_player.score < m_playerCharacterData.speedDownEndScore)
            {
                //float t = m_player.score / m_playerCharacterData.speedDownScoreMax;

                //downSpeed = (m_playerCharacterData.walkSpeed - m_playerCharacterData.walkMinSpeed) / (m_playerCharacterData.speedDownStartScore - m_playerCharacterData.speedDownEndScore);

                //downSpeed = m_player.score * -downSpeed;

                downSpeed = m_player.score * m_playerCharacterData.scoreWeight;
            }
            else if(m_player.score >= m_playerCharacterData.speedDownEndScore)
            {
                downSpeed = m_playerCharacterData.speedDownMax;
            }

            if (isDash)
            {
                if (m_rigidbody.velocity.magnitude > 0.25f &&
                    m_animatorStateInfo.fullPathHash != Animator.StringToHash("Base Layer.Movable.Tired"))
                {
                    m_dashGauge.gaugeRenderer.enabled   = true;
                    m_dashGauge.backRenderer.enabled    = true;
                    m_dashGauge.frameRenderer.enabled   = true;

                    m_sanddustParticle.Play();

                    m_movable.speed = m_playerCharacterData.dashSpeed - downSpeed;

                    m_dashDurationTime += Time.deltaTime;

                    if (m_dashDurationTime > m_playerCharacterData.dashTime)
                    {
                        m_sanddustParticle.Stop();

                        m_animator.SetTrigger(m_animatorParametersHashs[(int)AnimatorParametersID.Tired]);

                        m_dashDurationTime = m_playerCharacterData.dashTime;
                    }

                    m_bgmManager.PlaySELoop("se013_Dash");

                    m_isDash = true;
                }
            }
            else
            {
                m_sanddustParticle.Stop();

                m_movable.speed = m_playerCharacterData.walkSpeed - downSpeed;

                //m_dashDurationTime = 0.0f;

                //m_bgmManager.StopSE();

                //m_bgmManager.Stop
                //m_bgmManager.PlaySELoopSpatial("");

                if(m_beforeIsDash)
                    m_bgmManager.StopSE("se013_Dash");

                m_isDash = false;
            }
        }
    }

    public void InputRelease()
    {
        if(m_animatorStateInfo.fullPathHash == Animator.StringToHash("Base Layer.CaughtRibbon.Caught") &&
            m_caughtRibbon != null)
        {
            m_animator.SetTrigger(m_animatorParametersHashs[(int)AnimatorParametersID.InputRelease]);

            gameObject.layer = LayerMask.NameToLayer("PlayerCharacter");

            m_caughtRibbon.Breake();
            //m_caughtRibbon.Rebound();

            m_caughtRibbon = null;
        }        
    }

    public void InputRebound()
    {
        if (m_animatorStateInfo.fullPathHash == Animator.StringToHash("Base Layer.CaughtRibbon.Caught") &&
            m_caughtRibbon != null)
        {
            //m_animator.SetTrigger(m_animatorParametersHashs[(int)AnimatorParametersID.InputRelease]);

            //gameObject.layer = LayerMask.NameToLayer("PlayerCharacter");

            //m_caughtRibbon.Breake();
            m_caughtRibbon.Rebound();

            //m_caughtRibbon = null;

        }
    }

    public void InputCharge()
    {
        if (m_animatorParameters.isPushCancelKey == false)
        {
            m_animatorParameters.isPushThrowKey = true;

            //if(m_chargeTime >= m_playerCharacterData.chargeTimeMax)
            //{
            //    m_chargeTime = m_playerCharacterData.chargeTimeMax;
            //}
            //else
            //{
            //    m_chargeTime += Time.deltaTime;
            //}

            //float power = m_chargeTime / m_playerCharacterData.chargeTimeMax;

            //Debug.Log(power.ToString());

            //m_throwPower = power * m_playerCharacterData.throwPower;
            //m_throwSpeed = power * m_playerCharacterData.throwSpeed;
        }
    }

    public void InputCharge(float horizontal)
    {
        if (m_animatorParameters.isPushCancelKey == false)
        {
            m_animatorParameters.isPushThrowKey = true;

            m_throwPower = -horizontal * m_playerCharacterData.throwPower;
            m_throwSpeed = -horizontal * m_playerCharacterData.throwSpeed;
        }
    }

    public void InputThrow()
    {
        m_animatorParameters.isPushThrowKey = false;
    }

    public void InputThrow(bool isPush)
    {
        m_animatorParameters.isPushThrowKey = isPush;
    }

    public void InputHold()
    {
        m_animatorParameters.isPushHoldKey = true;
    }

    public void InputHold(bool isPush)
    {
        m_animatorParameters.isPushHoldKey = isPush;
    }

    public void InputPull()
    {
        if (m_animatorStateInfo.fullPathHash == Animator.StringToHash("Base Layer.Throw.Pull") || m_animatorStateInfo.fullPathHash == Animator.StringToHash("Base Layer.Throw.Pulling"))
        {
            if (m_controlledRibbon != null)
            {
                m_controlledRibbon.Pull(transform.position, m_playerCharacterData.ribbonPullPower);

                //StartCoroutine("PullAnimationCorutine");
                //StartCoroutine(PullAnimationCorutine());
                //m_animator.Play("Base Layer.Throw.Pull", 0, 0.0f);
                m_animator.SetTrigger("pull");

                m_bgmManager.PlaySE("se005_CatchRibbon");
            }
        }
    }

    public IEnumerator PullAnimationCorutine()
    {
        m_animator.speed = 1.0f;

        yield return new WaitForSeconds(1.0f);

        m_animator.speed = 0.01f;
    }

    public void InputCancel()
    {
        if (m_animatorStateInfo.fullPathHash == Animator.StringToHash("Base Layer.Throw.SizeAdjust"))
        {
            //m_animator.SetTrigger(m_animatorParametersHashs[(int)AnimatorParametersID.InputCancel]);
            m_animatorParameters.isPushCancelKey = true;

            m_ribbonRandingProjection.SetActive(false);

            m_isDoCancel = true;
        }
    }

    public void InputCancel(bool isPush)
    {
        //m_animatorParameters.isPushCancelKey = isPush;

        if (m_animatorStateInfo.fullPathHash == Animator.StringToHash("Base Layer.Throw.SizeAdjust")
            && isPush)
        {
            //m_animator.SetTrigger(m_animatorParametersHashs[(int)AnimatorParametersID.InputCancel]);
            m_isDoCancel = true;
            m_animator.SetTrigger("cancel");
        }

        //m_animatorParameters.isPushCancelKey = isPush;
    }

    public void StartExit()
    {
        transform.FindChild("PlayerNumberIcon").transform.gameObject.SetActive(false);
    }

    public void TiredExit()
    {
        m_animator.ResetTrigger(m_animatorParametersHashs[(int)AnimatorParametersID.Tired]);
    }

    public void SizeAdjustEnter()
    {
        if(m_controlledRibbon)
        {
            Destroy(m_controlledRibbon.gameObject);
        }

        GameObject ribbonObject = Instantiate(m_ribbonObject, transform.position, transform.rotation) as GameObject;

        ribbonObject.tag                        = tag;
        m_controlledRibbon                      = ribbonObject.GetComponent<Ribbons.Ribbon>();
        //m_controlledRibbon.transform.position   = transform.position;
        m_controlledRibbon.playerCharacter      = this;
        m_lengthAdjustTime                      = .0f;
        ribbonObject.transform.position         += new Vector3(.0f, 4.5f, .0f);

        m_ribbonRandingProjection.SetActive(true);

        m_controlledRibbon.transform.FindChild("RibbonLine").GetComponent<RibbonLine>().startTransform = m_arm;

        m_controlledRibbon.transform.localScale = new Vector3(m_playerCharacterData.ribbonMinScale, m_controlledRibbon.transform.localScale.y, m_playerCharacterData.ribbonMinScale);
        m_ribbonRandingProjection.transform.localScale = new Vector3(m_playerCharacterData.ribbonMinScale, m_playerCharacterData.ribbonMinScale, 1) / 2.0f;

        Vector3 position = transform.position;

        position.y = m_controlledRibbon.transform.position.y;

        m_controlledRibbon.transform.position = position;

        m_chargeTime = 0.0f;

        // 念のためリセット
        m_animator.ResetTrigger(m_animatorParametersHashs[(int)AnimatorParametersID.IsRibbonLanding]);
        m_animator.ResetTrigger(m_animatorParametersHashs[(int)AnimatorParametersID.IsPulled]);
        m_animator.ResetTrigger(m_animatorParametersHashs[(int)AnimatorParametersID.IsBreak]);
        m_animator.ResetTrigger(m_animatorParametersHashs[(int)AnimatorParametersID.InputCancel]);
        m_animator.ResetTrigger("cancel");

        m_isDoCancel = false;

        Assert.IsNotNull(ribbonObject);
        Assert.IsNotNull(m_controlledRibbon);
    }

    public void SizeAdjustUpdate()
    {
        if (m_chargeTime >= m_playerCharacterData.chargeTimeMax)
        {
            m_chargeTime = m_playerCharacterData.chargeTimeMax;
        }
        else
        {
            m_chargeTime += Time.deltaTime;
        }

        float power = m_chargeTime / m_playerCharacterData.chargeTimeMax;

        //Debug.Log(power.ToString());

        //m_throwPower = power * m_playerCharacterData.throwPower;
        m_throwPower = m_playerCharacterData.throwPower;
        m_throwSpeed = power * m_playerCharacterData.throwSpeed;

        m_lengthAdjustTime += Time.deltaTime;

        float t = m_lengthAdjustTime / m_playerCharacterData.ribbonSizeScailingTime;

        float ribbonSize = Mathf.PingPong(t, m_playerCharacterData.ribbonMaxScale - m_playerCharacterData.ribbonMinScale) + m_playerCharacterData.ribbonMinScale;

        m_controlledRibbon.transform.localScale = new Vector3(ribbonSize, m_controlledRibbon.transform.localScale.y, ribbonSize);

        //Vector3 force = Vector3.up * m_throwPower + transform.forward * m_throwSpeed;
        Vector3 force = transform.forward * m_throwSpeed;

        //force *= 2.0f;

        Vector3 start = transform.position + transform.forward * 3.0f;

        start.y += 4.5f;

        //Vector3 point = TakashiCompany.Unity.Util.TrajectoryCalculate.Force(transform.position + new Vector3(.0f, 1.0f, 1.0f), force, m_controlledRibbon.rigidbody.mass, Physics.gravity, .0f, m_playerCharacterData.ribbonProjectionTime);
        Vector3 point = TakashiCompany.Unity.Util.TrajectoryCalculate.Force(start, force, m_controlledRibbon.rigidbody.mass, Physics.gravity, 1.0f, m_playerCharacterData.ribbonProjectionTime);

        point.y = m_ribbonRandingProjection.transform.position.y;

        m_ribbonRandingProjection.transform.position    = point;
        m_ribbonRandingProjection.transform.localScale  = new Vector3(ribbonSize, ribbonSize, 1) / 1.25f;

        m_bgmManager.PlaySELoop("se000_AdjustRibbon");

        //m_vibrationLeft = Mathf.Sin(Mathf.PI * 2 / t);
        //m_vibrationRight = Mathf.Sin(Mathf.PI * 2 / t);

        //m_vibrationLeft = Mathf.PingPong(Time.time, 0.5f) + 0.0f;
        //m_vibrationRight = Mathf.PingPong(Time.time, 0.5f) + 0.0f;        

        Assert.IsNotNull(controlledRibbon);
    }

    public void SizeAdjustExit()
    {
        if(m_isDoCancel)
        {
            m_animatorParameters.isPushThrowKey = false;
            m_isDoCancel = false;
            m_ribbonRandingProjection.SetActive(false);

            if (m_controlledRibbon)
            {
                Destroy(m_controlledRibbon.gameObject);
                m_controlledRibbon = null;
            }

            m_animator.Play("Base Layer.Movable.Move");
        }
        else
        {
            m_ribbonRandingProjection.SetActive(false);
        }

        m_isDoCancel = false;

        m_chargeTime = 0.0f;
        m_vibrationLeft = 0.0f;
        m_vibrationRight = 0.0f;
    }

    public void LengthAdjustEnter()
    {
        //m_controlledRibbon.Throw(
        //        transform.position + new Vector3(.0f, 4.5f, 1.0f),
        //        transform.rotation,
        //        m_playerCharacterData.throwPower,
        //        m_playerCharacterData.throwSpeed);

        if(m_controlledRibbon == null)
        {
            m_animator.Play("Base Layer.Movable.Move");
            //"Base Layer.CaughtRibbon.Caught";
            m_isDoCancel = false;
            return;
        }

        Vector3 start = transform.position + transform.forward * 3.0f;

        start.y += 4.5f;

        m_controlledRibbon.Throw(
                start,
                transform.rotation,
                m_throwPower,
                m_throwSpeed);

        m_bgmManager.PlaySE("se001_ThrowJustRibbon");
        m_bgmManager.StopSE("se000_AdjustRibbon");
    }

    public void LengthAdjustUpdate()
    {
        //m_controlledRibbon.Move();
    }

    public void OnRibbonLanding()
    {
        m_animator.SetTrigger(m_animatorParametersHashs[(int)AnimatorParametersID.IsRibbonLanding]);
        m_sweatParticle.Play();

        m_bgmManager.PlaySE("se003_PutOnRibbon");
    }

    bool m_isPullEnterAfterFrame;

    public void PullEnter()
    {
        m_isPullEnterAfterFrame = true;
    }

    public void PullUpdate()
    {
        if(m_isPullEnterAfterFrame)
        {
            m_isPullEnterAfterFrame = false;

            //m_animator.speed = 0.01f;
        }

        if (m_controlledRibbon != null)
        {
            Vector3 vector = transform.position - m_controlledRibbon.transform.position;

            if(m_controlledRibbon.caughtObjectCount > 0)
            {
                switch (m_controlledRibbon.moveDirectionState1)
                {
                    case Ribbon.MoveDirectionState.Left:
                        m_vibrationLeft = 0.5f;
                        m_vibrationRight = 0.0f;
                        break;

                    case Ribbon.MoveDirectionState.Right:
                        m_vibrationLeft = 0.0f;
                        m_vibrationRight = 0.5f;
                        break;
                }
            }

            if (vector.magnitude < m_playerCharacterData.ribbonCollectLength)
            {
                m_animator.SetTrigger(m_animatorParametersHashs[(int)AnimatorParametersID.IsPulled]);

                m_controlledRibbon.Pulled();
                m_sweatParticle.Stop();
                m_controlledRibbon = null;

                m_vibrationLeft = 0.0f;
                m_vibrationRight = 0.0f;

                m_animator.speed = 1.0f;
            }
            else
            {
                Vector3 atPosition = m_controlledRibbon.transform.position;

                atPosition.y = 0;

                transform.LookAt(atPosition);
            }
        }
    }

    public void PullExit()
    {
        m_vibrationLeft = 0.0f;
        m_vibrationRight = 0.0f;
        m_animator.speed = 1.0f;
        //StopAllCoroutines();
    }

    public struct PulledCorutineArgs
    {
        public int addScore;
        public int inPlayerNum;
    }

    public void StartPulledCorutine(int score)
    {
        StartCoroutine("PulledCorutine", score);
      //  StartCoroutine("PulledCorutine");
    }

    public void StartPulledCorutine(int score, int inPlayerNum)
    {
        PulledCorutineArgs args;

        args.addScore = score;
        args.inPlayerNum = inPlayerNum;

        StartCoroutine("PulledCorutine", args);
        //  StartCoroutine("PulledCorutine");
    }

    public IEnumerator PulledCorutine(PulledCorutineArgs args)
    {
        const float OneLoopTime = 0.25f;

        float currentTime = 0.0f;

        while (currentTime < m_playerCharacterData.collectTime)
        {
            currentTime += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        currentTime = 0.0f;

        for (int i = 0; i < args.addScore; ++i)
        {
            currentTime = 0.0f;
            //m_vibrationLeft = 0.0f;
            //m_vibrationRight = 0.0f;

            while (currentTime < OneLoopTime)
            {
                currentTime += Time.deltaTime;

                float t = currentTime / OneLoopTime;

                // ease-in-out
                //t = (t * t) * (3.0f - (2.0f * t));

                float scaling;

                //float scaling = Mathf.PingPong(t, 1.0f) + 1.0f;
                scaling = Mathf.Sin(currentTime * 12.0f) * 0.35f;

                m_buildingObject.transform.localScale = m_buildingScale * (scaling + 1.0f);

                yield return new WaitForEndOfFrame();
            }

            m_player.score += 1;
            m_bgmManager.PlaySE("se015_InCampany");
            //m_vibrationLeft = 1.0f;
            //m_vibrationRight = 1.0f;

            StartCoroutine(PulledControllerCorutine());

            yield return new WaitForEndOfFrame();
        }

        for (int i = 0; i < args.inPlayerNum ; ++i)
        {
            currentTime = 0.0f;
            //m_vibrationLeft = 0.0f;
            //m_vibrationRight = 0.0f;

            while (currentTime < OneLoopTime)
            {
                currentTime += Time.deltaTime;

                float t = currentTime / OneLoopTime;

                // ease-in-out
                //t = (t * t) * (3.0f - (2.0f * t));

                float scaling;

                //float scaling = Mathf.PingPong(t, 1.0f) + 1.0f;
                scaling = Mathf.Sin(currentTime * 12.0f) * 0.35f;

                m_buildingObject.transform.localScale = m_buildingScale * (scaling + 1.0f);

                yield return new WaitForEndOfFrame();
            }

            //m_player.score += 1;
            m_bgmManager.PlaySE("se015_InCampany");

            //m_vibrationLeft = 1.0f;
            //m_vibrationRight = 1.0f;

            StartCoroutine(PulledControllerCorutine());

            yield return new WaitForEndOfFrame();
        }

        //m_vibrationLeft = 0.0f;
        //m_vibrationRight = 0.0f;

        m_isChangeBuildingSize = true;

        //m_buildingObject.transform.localScale = m_buildingScale;
    }

    public IEnumerator PulledControllerCorutine()
    {
        m_vibrationLeft     = 0.75f;
        m_vibrationRight    = 0.75f;

        for (float time = 0.0f; time < 0.125f; time += Time.deltaTime)
        {
            yield return new WaitForEndOfFrame();
        }

        m_vibrationLeft     = 0.0f;
        m_vibrationRight    = 0.0f;

        yield return null;
    }

    public IEnumerator PulledCorutine(int score)
    {
        const float OneLoopTime = 0.25f;

        float currentTime = 0.0f;

        while (currentTime < m_playerCharacterData.collectTime)
        {
            currentTime += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        currentTime = 0.0f;

        for (int i = 0; i < score; ++i)
        {
            currentTime = 0.0f;

            while (currentTime < OneLoopTime)
            {
                currentTime += Time.deltaTime;

                float t = currentTime / OneLoopTime;

                // ease-in-out
                //t = (t * t) * (3.0f - (2.0f * t));

                float scaling;

                //float scaling = Mathf.PingPong(t, 1.0f) + 1.0f;
                scaling = Mathf.Sin(currentTime * 12.0f) * 0.35f;

                m_buildingObject.transform.localScale = m_buildingScale * (scaling + 1.0f);

                yield return new WaitForEndOfFrame();
            }

            m_player.score += 1;
            m_bgmManager.PlaySE("se015_InCampany");
        }

        m_buildingObject.transform.localScale = m_buildingScale;
    }

    public IEnumerator PulledCorutineAAA()
    {
        const float OneLoopTime = .75f;

        float currentTime = 0.0f;

        m_bgmManager.PlaySE("se015_InCampany");

        while (currentTime < OneLoopTime)
        {
            currentTime += Time.deltaTime;

            float t = currentTime / OneLoopTime;

            // ease-in-out
            t = (t * t) * (3.0f - (2.0f * t));

            float scaling = Mathf.PingPong(0.5f, t) + 1.0f;

            m_buildingObject.transform.localScale = m_buildingScale * scaling;

            yield return null;
        }

        m_buildingObject.transform.localScale = m_buildingScale;
    }

    public void BreakeRibbon()
    {
        if(m_controlledRibbon)
        {
            m_animator.SetTrigger(m_animatorParametersHashs[(int)AnimatorParametersID.IsBreak]);
            m_sweatParticle.Stop();
            //Destroy(m_controlledRibbon.gameObject);
            m_controlledRibbon = null;
            m_playerIcon.ChangeIconNormal();
            m_animator.speed = 1.0f;
        }
    }

    public void CaughtRibbon(Ribbon caughtRibbon)
    {
        //if(isCaught)
        //if(m_animatorStateInfo.fullPathHash != Animator.StringToHash("Base Layer.CaughtRibbon.Caught") &&
        //    m_animatorStateInfo.fullPathHash != Animator.StringToHash("Base Layer.CaughtRibbon.Release") &&
        //    m_animatorStateInfo.fullPathHash != Animator.StringToHash("Base Layer.Movable.Invisible"))
        {
            //@todo Change SetTrigger
            m_animator.Play("Base Layer.CaughtRibbon.Caught");

            gameObject.layer = LayerMask.NameToLayer("CaughtPlayerCharacter");
            m_caughtRibbon = caughtRibbon;
            m_playerIcon.ChangeIconAngry();

            if (m_controlledRibbon)
            {
                //m_animator.SetTrigger("isBreaked");

                //m_controlledRibbon.Breake();

                //Destroy(m_controlledRibbon.gameObject);

                {
                    m_controlledRibbon.BreakeTriggerNoAbsorption();
                }

                m_controlledRibbon = null;
            }

            m_isThisFrameCought = true;

            //m_lineRenderer.enabled = true;

            m_lineRenderer.SetPosition(1, caughtRibbon.playerCharacter.transform.position);
            m_lineRenderer.material = caughtRibbon.playerCharacter.ribbonLineMaterial;

            m_rollRibbonRenderer.enabled    = true;

            m_rollRibbonRenderer.materials[0] = caughtRibbon.playerCharacter.ribbonMaterials[caughtRibbon.playerCharacter.GetPlayerNumber() - 1];
            m_rollRibbonRenderer.materials[1] = caughtRibbon.playerCharacter.ribbonMaterials[caughtRibbon.playerCharacter.GetPlayerNumber() - 1];
            m_rollRibbonRenderer.material = caughtRibbon.playerCharacter.ribbonMaterials[caughtRibbon.playerCharacter.GetPlayerNumber() - 1];


            transform.LookAt(caughtRibbon.playerCharacter.transform);
        }
    }

    public void CaughtExit()
    {
        m_isThisFrameCought = false;
    }

    public void CatchRelease()
    {
        m_animator.SetTrigger(m_animatorParametersHashs[(int)AnimatorParametersID.InputRelease]);
        //m_animator.Play("");

        gameObject.layer = LayerMask.NameToLayer("PlayerCharacter");

        m_playerIcon.ChangeIconNormal();

        m_lineRenderer.enabled = false;

        //m_rigidbody.mass = 1.0f;
        m_rollRibbonRenderer.enabled = false;

        if(Mathf.Abs(transform.position.x) > 32.5f || Mathf.Abs(transform.position.z) > 22.5f)
        {
            transform.position = Vector3.zero;
        }
    }

    public void Collect()
    {
        m_animator.SetTrigger(m_animatorParametersHashs[(int)AnimatorParametersID.IsCollect]);

        gameObject.layer = LayerMask.NameToLayer("PlayerCharacter");
        m_collectTime = .0f;
        //m_rigidbody.mass = 1.0f;

        m_playerIcon.ChangeIconSad();

        m_playerAbsorption.startAbsorption(transform.position, m_caughtRibbon.playerCharacter.transform.position);

        m_collider.enabled      = false;
        m_wallCollider.enabled  = false;
        m_lineRenderer.enabled  = false;
        m_isCollected = true;
    }

    public void CollectUpdate()
    {
        m_collectTime += Time.deltaTime;

        if(m_collectTime > m_playerCharacterData.collectTime)
        {
            m_animator.SetTrigger(m_animatorParametersHashs[(int)AnimatorParametersID.InBuilding]);

            m_inBuildingTime = .0f;
        }

        m_playerAbsorption.SetEndPosition(m_caughtRibbon.playerCharacter.transform.position);
        transform.position = m_playerAbsorption.GetLerpPointAtTime();

        // test
        //m_meshObject.SetActive(false);
        //m_buildingObject.SetActive(false);

        transform.localScale = Vector3.Lerp(m_dafaultScale, Vector3.zero, m_collectTime / m_playerCharacterData.collectTime);
    }

    public void KnockbackEnter()
    {
        m_rigidbody.AddForce((transform.forward * -1 )* m_playerCharacterData.knockbackPower);
    }

    public void InBuildingEnter()
    {
        // test
        m_meshObject.SetActive(false);
        m_buildingObject.SetActive(false);
        m_collider.enabled = false;
        transform.localScale = m_dafaultScale;
        m_isChangeBuildingSize = true;
        m_rollRibbonRenderer.enabled = false;

        m_inBuildingTime = .0f;
    }

    public void InBuildingUpdate()
    {
        m_inBuildingTime += Time.deltaTime;

        if(m_inBuildingTime > m_playerCharacterData.inBuildingTime)
        {
            m_animator.SetTrigger(m_animatorParametersHashs[(int)AnimatorParametersID.OutBuilding]);
        }

        transform.position = m_caughtRibbon.playerCharacter.transform.position;
    }

    public void InBuildingExit()
    {
        m_meshObject.SetActive(true);
        m_buildingObject.SetActive(true);

        m_caughtRibbon.playerCharacter.playerFire.Fire(transform, m_rigidbody);

        //m_rigidbody.mass = 0.1f;

        m_playerIcon.ChangeIconNormal();

        m_wallCollider.enabled = true;
        Color invisibleColor = new Color(1, 1, 1, 0.7f);
        for (int i = 0; i < m_meshRenderers.Length; ++i)
        {
            m_meshRenderers[i].material = m_invisibleMaterial;
            m_meshRenderers[i].material.SetColor("_Color", invisibleColor);
        }

        m_isCollected = false;
    }

    //public void OutBuildingEnter()
    //{
    //    m_rigidbody.mass = 1.0f;
    //}

    public void OutBuildingUpdate()
    {
        if (m_rigidbody.velocity.y <= 0.0f)
        {
            int raycastLayerMask = LayerMask.GetMask(new string[] { "Stage" });

            bool isGrounded = Physics.Raycast(
            transform.position,
            Vector3.down,
            .5f,
            raycastLayerMask);

            if (isGrounded)
            {
                m_collider.enabled = true;

                m_animator.SetTrigger(m_animatorParametersHashs[(int)AnimatorParametersID.OutBuildingExit]);

              //  m_rigidbody.mass = 0.1f;
            }
        }
    }

    public void InvisibleEnter()
    {
        m_invisibleTime = 0.0f;

        //foreach (var meshRenderer in m_meshRenderers)
        //{
        //    meshRenderer.materials[0] = m_invisibleMaterial;
        //}
        
    }

    public void InvisibleUpdate()
    {
        m_invisibleTime += Time.deltaTime;

        Color invisibleColor = new Color(1,1,1,Mathf.PingPong(Time.time*15,1));
        for (int i = 0; i < m_meshRenderers.Length; ++i)
        {
            m_meshRenderers[i].material.SetColor("_Color",invisibleColor);
        }


        if(m_invisibleTime > m_playerCharacterData.invisibleTime)
        {
            m_animator.SetTrigger(m_animatorParametersHashs[(int)AnimatorParametersID.InvisibleEnd]);

            //foreach (var meshRenderer in m_meshRenderers)
            //{
            //    meshRenderer.materials[0] = m_origineMaterial;
            //}

            for (int i = 0; i < m_meshRenderers.Length; ++i)
            {
                m_meshRenderers[i].material = m_origineMaterial;
            }
        }
    }

    public void OnHoldEnter()
    {
        Ray         ray         = new Ray(transform.position, transform.forward);
        //int         layerMask   = LayerMask.GetMask(new string[] { "PlayerCharacterBuilding" });
        int layerMask = LayerMask.GetMask(new string[] { "PlayerCharacterBuilding", "Girl" });
        var rayCastHits = Physics.SphereCastAll(ray, m_collider.radius, 1.5f, layerMask);   //
        float       minDistance = 1.5f; //
        GameObject  gameObject  = null;

        GameObject playerCharacterObject = null;

        foreach(var rayCastHit in rayCastHits)
        {
            if (rayCastHit.distance < minDistance)
            {
                minDistance = rayCastHit.distance;

                gameObject = rayCastHit.collider.gameObject;

                if (gameObject.layer == LayerMask.NameToLayer("PlayerCharacterBuiling"))
                {
                    playerCharacterObject = gameObject;
                }
            }
        }

        if(playerCharacterObject != null)
        {
            m_holdingPlayerCharacter = playerCharacterObject.GetComponent<PlayerCharacter>();

            m_holdingPlayerCharacter.Hold(this);

            m_animator.SetTrigger(m_animatorParametersHashs[(int)AnimatorParametersID.HoldPlayer]);
        }
        else
        {
            // Girl
            m_animator.SetTrigger(m_animatorParametersHashs[(int)AnimatorParametersID.HoldGirl]);

            m_player.score += 1;

            m_holdingGirl = gameObject.GetComponent<GirlNoPlayerCharacter>();
            //m_holdingGirl.Hold();
        }
    }

    public void OnHoldingPlayerCharacter()
    {
        
    }

    public void ShakingUpdate()
    {
        m_shakingTime += Time.deltaTime;

        float t = m_shakingTime / m_playerCharacterData.shakingTime;

        
    }

    public void OnHoldGirl()
    {
        
    }

    public void Shake()
    {
        
    }

    IEnumerator ShakeCorutine()
    {
        float releaseAngleOffset;

        releaseAngleOffset = 180.0f / m_playerCharacterData.shakingReleaseGirl;

        Quaternion releaseAngle = Quaternion.Euler(.0f, releaseAngleOffset, .0f);

        float median = m_playerCharacterData.shakingReleaseGirl / 2.0f;

        for (int i = 0; i < m_playerCharacterData.shakingRepeat; ++i)
        {
            for (int j = 0; j < m_playerCharacterData.shakingReleaseGirl; ++j)
            {
                releaseAngle = Quaternion.Euler(.0f, releaseAngleOffset * (median + j), .0f);

                releaseAngle = Quaternion.Euler(.0f, releaseAngleOffset * (median - j), .0f);
            }

            yield return new WaitForSeconds(m_playerCharacterData.shakingInterval);
        }

        yield return null;
    }

    public void Hold(PlayerCharacter holdedPlayerCharacter)
    {
        m_holdedPlayerCharacter = holdedPlayerCharacter;

        m_animator.Play("Base Layer.Holded.Holding");
    }

    public void KnockBack(Vector3 forceDirection)
    {
        m_animator.SetTrigger(m_animatorParametersHashs[(int)AnimatorParametersID.HoldGirl]);
    }

    void Awake()
    {
        m_playerCharacterData   = Resources.Load(m_playerCharacterDataPath)                 as PlayerCharacterData;
        m_ribbonObject          = Resources.Load(m_playerCharacterData.ribbonPrefabPath)    as GameObject;

        Assert.IsNotNull(m_playerCharacterData);
        Assert.IsNotNull(m_ribbonObject);
    }

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_animator  = GetComponent<Animator>();
        m_movable   = GetComponent<Movable>();
        m_collider  = GetComponent<CapsuleCollider>();
        m_wallCollider = GetComponent<BoxCollider>();
        m_player    = transform.parent.GetComponent<Player>();
        m_lineRenderer = GetComponent<LineRenderer>();

        m_meshObject                = transform.FindChild("PlayerCharacterMesh").gameObject;
        m_buildingObject            = transform.FindChild("PlayerCharacterBuilding").gameObject;
        m_ribbonRandingProjection   = transform.FindChild("RibbonLandingProjection").gameObject;

        m_playerFire                = transform.FindChild("PlayerCharacterBuilding").gameObject.GetComponent<PlayerFire>();
        m_dashGauge                 = transform.FindChild("Gauge").FindChild("DashGaugeMain").gameObject.GetComponent<DashGauge>();

        m_sweatParticle             = transform.FindChild("Ase").gameObject.GetComponent<ParticleSystem>();
        m_sanddustParticle          = transform.FindChild("SandDust").gameObject.GetComponent<ParticleSystem>();
        m_npcGetParticle            = transform.FindChild("NpcGetEffect").gameObject.GetComponent<ParticleSystem>();

        m_playerAbsorption         = transform.FindChild("CharacterAbsorption").gameObject.GetComponent<PlayerAbsorption>();

        m_rollRibbonRenderer        = transform.FindChild("RollRibbon").GetComponent<MeshRenderer>();

        //m_damage                    = GameObject.Find()

        m_bgmManager                = BGMManager.instance;

        m_dafaultScale = transform.localScale;
        m_buildingScale = m_buildingObject.transform.localScale;


        var playerIcons = FindObjectsOfType<PlayerIcon>();

        foreach(var playerIcon in playerIcons)
        {
            if(playerIcon.gameObject.tag == tag)
            {
                m_playerIcon = playerIcon;
                break;
            }
        }

        _InitializeAnimatorParametersID();
        _InitializeAnimationState();

        Assert.IsNotNull(m_animator);
        Assert.IsNotNull(m_movable);
        Assert.IsNotNull(m_dashGauge);
    }

    void Update()
    {
        m_animatorStateInfo = m_animator.GetCurrentAnimatorStateInfo(0);

        if(!m_isDash)
        {
            if (m_dashDurationTime > 0.0f)
            {
                m_dashDurationTime -= Time.deltaTime;

                if (m_dashDurationTime <= 0.0f)
                {
                    m_dashGauge.gaugeRenderer.enabled = false;
                    m_dashGauge.backRenderer.enabled = false;
                    m_dashGauge.frameRenderer.enabled = false;
                }
            }
            //else
                //m_dashDurationTime = .0f;
        }

        //if(m_controlledRibbon)
        //{
        //    switch(m_controlledRibbon.moveDirectionState1)
        //    {
        //        case Ribbon.MoveDirectionState.Left:
        //            m_vibrationLeft = 1.0f;
        //            m_vibrationRight = 0.25f;
        //            break;

        //        case Ribbon.MoveDirectionState.Right:
        //            m_vibrationLeft = 0.25f;
        //            m_vibrationRight = 1.0f;
        //            break;
        //    }
        //}

        GamePad.SetVibration(m_playerIndex, m_vibrationLeft, m_vibrationRight);

        m_dashGauge.raito = 1 - (m_dashDurationTime / m_playerCharacterData.dashTime);

        m_lineRenderer.SetPosition(0, transform.position);

        m_buildingScale.y = 3 + (m_player.score * 0.25f);

        if (m_isChangeBuildingSize)
        {
            m_buildingObject.transform.localScale = m_buildingScale;

            m_isChangeBuildingSize = false;
        }

        _UpdateAnimatorParameters();
    }

    void OnDestroy()
    {
        GamePad.SetVibration(m_playerIndex, .0f, .0f);
    }

    private enum AnimatorParametersID
    {
        IsDownStick = 0,
        IsPushThrowKey,
        IsPushHoldKey,
        IsPushCancelKey,
        IsRibbonLanding,
        IsPulled,
        InputRelease,
        IsCollect,
        IsBreak,
        Velocity,
        InBuilding,
        OutBuilding,
        OutBuildingExit,
        InputCancel,
        HoldPlayer,
        HoldGirl,
        Shake,
        Knockback,
        Tired,
        InvisibleEnd,
    }

    private struct AnimatorParameters
    {
        public bool isDownStick;
        public bool isPushThrowKey;
        public bool isPushHoldKey;
        public bool isPushCancelKey;
        public float velocity;
    }

    private void _InitializeAnimatorParametersID()
    {
        int arraySize = Enum.GetValues(typeof(AnimatorParametersID)).Length;

        m_animatorParametersHashs = new int[arraySize];

        m_animatorParametersHashs[(int)AnimatorParametersID.IsDownStick]        = Animator.StringToHash("isDownStick");
        m_animatorParametersHashs[(int)AnimatorParametersID.IsPushThrowKey]     = Animator.StringToHash("isPushThrowKey");
        m_animatorParametersHashs[(int)AnimatorParametersID.IsPushHoldKey]      = Animator.StringToHash("isPushHoldKey");
        m_animatorParametersHashs[(int)AnimatorParametersID.IsPushCancelKey]    = Animator.StringToHash("isPushCancelKey");
        m_animatorParametersHashs[(int)AnimatorParametersID.IsRibbonLanding]    = Animator.StringToHash("isRibbonLanding");
        m_animatorParametersHashs[(int)AnimatorParametersID.IsPulled]           = Animator.StringToHash("isPulled");
        m_animatorParametersHashs[(int)AnimatorParametersID.InputRelease]       = Animator.StringToHash("inputRelease");
        m_animatorParametersHashs[(int)AnimatorParametersID.IsCollect]          = Animator.StringToHash("isCollect");
        m_animatorParametersHashs[(int)AnimatorParametersID.IsBreak]            = Animator.StringToHash("isBreak");
        m_animatorParametersHashs[(int)AnimatorParametersID.Velocity]           = Animator.StringToHash("velocity");
        m_animatorParametersHashs[(int)AnimatorParametersID.InBuilding]         = Animator.StringToHash("inBuilding");
        m_animatorParametersHashs[(int)AnimatorParametersID.OutBuilding]        = Animator.StringToHash("outBuilding");
        m_animatorParametersHashs[(int)AnimatorParametersID.OutBuildingExit]    = Animator.StringToHash("outBuildingExit");
        m_animatorParametersHashs[(int)AnimatorParametersID.InputCancel]        = Animator.StringToHash("inputCancel");
        m_animatorParametersHashs[(int)AnimatorParametersID.HoldPlayer]         = Animator.StringToHash("holdPlayer");
        m_animatorParametersHashs[(int)AnimatorParametersID.HoldGirl]           = Animator.StringToHash("holdGirl");
        m_animatorParametersHashs[(int)AnimatorParametersID.Knockback]          = Animator.StringToHash("knockback");
        m_animatorParametersHashs[(int)AnimatorParametersID.Tired]              = Animator.StringToHash("tired");
        m_animatorParametersHashs[(int)AnimatorParametersID.InvisibleEnd]       = Animator.StringToHash("invisibleEnd");
    }

    private void _InitializeAnimationState()
    {
        int arraySize = Enum.GetValues(typeof(MainState)).Length;

        m_mainStateHashs = new int[arraySize];

        m_mainStateHashs[(int)MainState.Start]          = Animator.StringToHash("Start");
        m_mainStateHashs[(int)MainState.Movable]        = Animator.StringToHash("Movable");
        m_mainStateHashs[(int)MainState.Throw]          = Animator.StringToHash("Throw");
        m_mainStateHashs[(int)MainState.Hold]           = Animator.StringToHash("Hold");
        m_mainStateHashs[(int)MainState.CaughtRibbon]   = Animator.StringToHash("CaughtRibbon");
        m_mainStateHashs[(int)MainState.CaughtHold]     = Animator.StringToHash("CaughtHold");

        arraySize = Enum.GetValues(typeof(MovableState)).Length;

        m_movableStateHashs = new int[arraySize];

        m_movableStateHashs[(int)MovableState.Wait] = Animator.StringToHash("Base Layer.Movable.Wait");
        m_movableStateHashs[(int)MovableState.Move] = Animator.StringToHash("Base Layer.Movable.Move");
        m_movableStateHashs[(int)MovableState.Exit] = Animator.StringToHash("Base Layer.Movable.Exit");

        arraySize = Enum.GetValues(typeof(ThrowState)).Length;

        m_throwStateHashs = new int[arraySize];

        m_throwStateHashs[(int)ThrowState.SizeAdjust]     = Animator.StringToHash("Base Layer.Movable.SizeAdjust");
        m_throwStateHashs[(int)ThrowState.LengthAdjust]   = Animator.StringToHash("Base Layer.Movable.LengthAdjust");
        m_throwStateHashs[(int)ThrowState.Pull]           = Animator.StringToHash("Base Layer.Movable.Pull");
        m_throwStateHashs[(int)ThrowState.Collect]        = Animator.StringToHash("Base Layer.Movable.Collect");
    }

    private void _UpdateAnimatorParameters()
    {
        m_animatorParameters.velocity = m_rigidbody.velocity.magnitude;

        m_animator.SetBool(m_animatorParametersHashs[(int)AnimatorParametersID.IsDownStick],        m_animatorParameters.isDownStick);
        m_animator.SetBool(m_animatorParametersHashs[(int)AnimatorParametersID.IsPushThrowKey],     m_animatorParameters.isPushThrowKey);
        m_animator.SetBool(m_animatorParametersHashs[(int)AnimatorParametersID.IsPushHoldKey],      m_animatorParameters.isPushHoldKey);
        m_animator.SetBool(m_animatorParametersHashs[(int)AnimatorParametersID.IsPushCancelKey],    m_animatorParameters.isPushCancelKey);
        m_animator.SetFloat(m_animatorParametersHashs[(int)AnimatorParametersID.Velocity],          m_animatorParameters.velocity);
    }

    [SerializeField, Tooltip("")]
    private string m_playerCharacterDataPath;

    private Rigidbody m_rigidbody;

    public new Rigidbody rigidbody
    {
        get
        {
            return m_rigidbody;
        }

        set
        {
            m_rigidbody = value;
        }
    }

    private Movable m_movable;

    private GameObject m_ribbonObject;

    private Ribbon m_controlledRibbon;

    public Ribbon controlledRibbon
    {
        get
        {
            return m_controlledRibbon;
        }

        set
        {
            m_controlledRibbon = value;
        }
    }

    public Ribbon caughtRibbon
    {
        get
        {
            return m_caughtRibbon;
        }

        set
        {
            m_caughtRibbon = value;
        }
    }

    private Ribbon m_caughtRibbon;

    private float m_lengthAdjustTime;

    private float m_collectTime;

    private float m_inBuildingTime;

    private float m_dashDurationTime;

    private Player m_player;

    public Player player
    {
        get
        {
            return m_player;
        }

        set
        {
            m_player = value;
        }
    }

    private PlayerCharacter m_holdingPlayerCharacter;

    private GirlNoPlayerCharacter m_holdingGirl;

    private PlayerCharacter m_holdedPlayerCharacter;

    private float m_shakingTime;

    private PlayerCharacterData m_playerCharacterData;
    
    public PlayerCharacterData playerCharacterData
    {
        get { return m_playerCharacterData; }
    }

    private CapsuleCollider m_collider;

    private GameObject m_meshObject;

    private GameObject m_buildingObject;

    private GameObject m_ribbonRandingProjection;

    private Animator m_animator;

    private AnimatorParameters m_animatorParameters;

    private AnimatorStateInfo m_animatorStateInfo;

    private int[] m_animatorParametersHashs;

    private int[] m_mainStateHashs;

    private int[] m_movableStateHashs;

    private int[] m_throwStateHashs;

    public bool isCaught
    {
        get
        {
            return !(m_animatorStateInfo.fullPathHash == Animator.StringToHash("Base Layer.CaughtRibbon.Caught") ||
                m_animatorStateInfo.fullPathHash == Animator.StringToHash("Base Layer.CaughtRibbon.Collect"));
                //!(m_animatorStateInfo.shortNameHash == Animator.StringToHash("Caught") ||
                //m_animatorStateInfo.shortNameHash == Animator.StringToHash("Collect"));
        }
    }

    private PlayerFire m_playerFire;

    public PlayerFire playerFire
    {
        get
        {
            return m_playerFire;
        }

        set
        {
            m_playerFire = value;
        }
    }

    private ParticleSystem m_sweatParticle;

    private ParticleSystem m_sanddustParticle;

    private ParticleSystem m_npcGetParticle;

    public ParticleSystem npcGetParticle
    {
        get
        {
            return m_npcGetParticle;
        }
    }

    public Material[] ribbonMaterials
    {
        get
        {
            return m_ribbonMaterials;
        }

        set
        {
            m_ribbonMaterials = value;
        }
    }

    public AnimatorStateInfo animatorStateInfo
    {
        get
        {
            return m_animatorStateInfo;
        }

        set
        {
            m_animatorStateInfo = value;
        }
    }

    public bool isThisFrameCought
    {
        get
        {
            return m_isThisFrameCought;
        }

        set
        {
            m_isThisFrameCought = value;
        }
    }

 //   public bool isCa

    public Material ribbonLineMaterial
    {
        get
        {
            return m_ribbonLineMaterial;
        }

        set
        {
            m_ribbonLineMaterial = value;
        }
    }

    public PlayerIndex playerIndex
    {
        get
        {
            return m_playerIndex;
        }

        set
        {
            m_playerIndex = value;
        }
    }

    public Damage damage
    {
        get
        {
            return m_damage;
        }

        set
        {
            m_damage = value;
        }
    }

    public bool m_isCollected { get; private set; }

    [SerializeField]
    bool m_isDoCancel;

    bool m_isDash;

    DashGauge m_dashGauge;

    [SerializeField]
    Material[] m_ribbonMaterials;

    BGMManager m_bgmManager;

    PlayerIcon m_playerIcon;

    PlayerAbsorption m_playerAbsorption;

    Vector3 m_dafaultScale;

    BoxCollider m_wallCollider;

    bool m_isThisFrameCought;

    float m_invisibleTime;

    [SerializeField]
    SkinnedMeshRenderer[] m_meshRenderers;

    [SerializeField]
    Material m_invisibleMaterial;

    [SerializeField]
    Material m_origineMaterial;

    [SerializeField]
    Material m_ribbonLineMaterial;

    [SerializeField]
    Transform m_arm;

    Vector3 m_buildingScale;

    LineRenderer m_lineRenderer;

    bool m_isChangeBuildingSize;

    float m_chargeTime;

    float m_throwPower;
    float m_throwSpeed;

    MeshRenderer m_rollRibbonRenderer;

    PlayerIndex m_playerIndex;

    float m_vibrationLeft;
    float m_vibrationRight;

    [SerializeField]
    Damage m_damage;

    bool m_beforeIsDash;
}
