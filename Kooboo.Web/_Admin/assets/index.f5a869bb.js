var he=Object.defineProperty;var se=Object.getOwnPropertySymbols;var _e=Object.prototype.hasOwnProperty,Ce=Object.prototype.propertyIsEnumerable;var ae=(e,o,t)=>o in e?he(e,o,{enumerable:!0,configurable:!0,writable:!0,value:t}):e[o]=t,U=(e,o)=>{for(var t in o||(o={}))_e.call(o,t)&&ae(e,t,o[t]);if(se)for(var t of se(o))Ce.call(o,t)&&ae(e,t,o[t]);return e};import{E as ue}from"./index.2bc50276.js";import{E as ye,O as Te}from"./index.379e80ad.js";import{E as Fe}from"./index.a439b087.js";import{b as $e,d as Q,c as E,_ as M,E as ce,t as ke,u as ee,R as Re,q as Se,w as Oe,r as pe}from"./windi.a5b0b048.js";import{c as Ne,E as Pe,d as De,a as fe,C as Be,b as Ke,e as Ge,f as Me,g as Ae,F as Le,L as ze}from"./dropdown.0507a1c7.js";import{o as Ue,a as Ye,c as O,w as ie}from"./event.53b2ad83.js";import{d as A,g as C,B as _,c as T,b2 as J,ao as He,as as Y,u as y,i as Ve,h as Je,aW as We,r as P,N as b,o as N,j as L,w as v,b as R,au as je,aN as qe,K as Qe,A as me,a as W,aF as Xe,aT as H,n as Z,k as x,f as Ze,l as xe,F as eo,m as oo}from"./url.2e6a77c4.js";import{u as no}from"./index.fcc3ea42.js";import{b as ve}from"./index.ff0264f3.js";import{c as ge}from"./refs.4001ce17.js";import{F as to}from"./focus-trap.23f44899.js";const ro=$e({style:{type:Q([String,Array,Object])},currentTabId:{type:Q(String)},defaultCurrentTabId:String,loop:Boolean,dir:{type:String,values:["ltr","rtl"],default:"ltr"},orientation:{type:Q(String)},onBlur:Function,onFocus:Function,onMousedown:Function}),{ElCollection:lo,ElCollectionItem:so,COLLECTION_INJECTION_KEY:oe,COLLECTION_ITEM_INJECTION_KEY:ao}=Ne("RovingFocusGroup"),ne=Symbol("elRovingFocusGroup"),we=Symbol("elRovingFocusGroupItem"),io={ArrowLeft:"prev",ArrowUp:"prev",ArrowRight:"next",ArrowDown:"next",PageUp:"first",Home:"first",PageDown:"last",End:"last"},uo=(e,o)=>{if(o!=="rtl")return e;switch(e){case E.right:return E.left;case E.left:return E.right;default:return e}},co=(e,o,t)=>{const r=uo(e.key,t);if(!(o==="vertical"&&[E.left,E.right].includes(r))&&!(o==="horizontal"&&[E.up,E.down].includes(r)))return io[r]},po=(e,o)=>e.map((t,r)=>e[(r+o)%e.length]),te=e=>{const{activeElement:o}=document;for(const t of e)if(t===o||(t.focus(),o!==document.activeElement))return},de="currentTabIdChange",X="rovingFocusGroup.entryFocus",fo={bubbles:!1,cancelable:!0},mo=A({name:"ElRovingFocusGroupImpl",inheritAttrs:!1,props:ro,emits:[de,"entryFocus"],setup(e,{emit:o}){var t;const r=C((t=e.currentTabId||e.defaultCurrentTabId)!=null?t:null),d=C(!1),c=C(!1),l=C(null),{getItems:s}=_(oe,void 0),p=T(()=>[{outline:"none"},e.style]),g=a=>{o(de,a)},w=()=>{d.value=!0},I=O(a=>{var f;(f=e.onMousedown)==null||f.call(e,a)},()=>{c.value=!0}),h=O(a=>{var f;(f=e.onFocus)==null||f.call(e,a)},a=>{const f=!y(c),{target:D,currentTarget:F}=a;if(D===F&&f&&!y(d)){const B=new Event(X,fo);if(F==null||F.dispatchEvent(B),!B.defaultPrevented){const m=s().filter(k=>k.focusable),S=m.find(k=>k.active),$=m.find(k=>k.id===y(r)),G=[S,$,...m].filter(Boolean).map(k=>k.ref);te(G)}}c.value=!1}),n=O(a=>{var f;(f=e.onBlur)==null||f.call(e,a)},()=>{d.value=!1}),i=(...a)=>{o("entryFocus",...a)};J(ne,{currentTabbedId:He(r),loop:Y(e,"loop"),tabIndex:T(()=>y(d)?-1:0),rovingFocusGroupRef:l,rovingFocusGroupRootStyle:p,orientation:Y(e,"orientation"),dir:Y(e,"dir"),onItemFocus:g,onItemShiftTab:w,onBlur:n,onFocus:h,onMousedown:I}),Ve(()=>e.currentTabId,a=>{r.value=a!=null?a:null}),Je(()=>{const a=y(l);Ue(a,X,i)}),We(()=>{const a=y(l);Ye(a,X,i)})}});function vo(e,o,t,r,d,c){return P(e.$slots,"default")}var go=M(mo,[["render",vo],["__file","/home/runner/work/element-plus/element-plus/packages/components/roving-focus-group/src/roving-focus-group-impl.vue"]]);const wo=A({name:"ElRovingFocusGroup",components:{ElFocusGroupCollection:lo,ElRovingFocusGroupImpl:go}});function bo(e,o,t,r,d,c){const l=b("el-roving-focus-group-impl"),s=b("el-focus-group-collection");return N(),L(s,null,{default:v(()=>[R(l,je(qe(e.$attrs)),{default:v(()=>[P(e.$slots,"default")]),_:3},16)]),_:3})}var Io=M(wo,[["render",bo],["__file","/home/runner/work/element-plus/element-plus/packages/components/roving-focus-group/src/roving-focus-group.vue"]]);const Eo=A({components:{ElRovingFocusCollectionItem:so},props:{focusable:{type:Boolean,default:!0},active:{type:Boolean,default:!1}},emits:["mousedown","focus","keydown"],setup(e,{emit:o}){const{currentTabbedId:t,loop:r,onItemFocus:d,onItemShiftTab:c}=_(ne,void 0),{getItems:l}=_(oe,void 0),s=ve(),p=C(null),g=O(n=>{o("mousedown",n)},n=>{e.focusable?d(y(s)):n.preventDefault()}),w=O(n=>{o("focus",n)},()=>{d(y(s))}),I=O(n=>{o("keydown",n)},n=>{const{key:i,shiftKey:a,target:f,currentTarget:D}=n;if(i===E.tab&&a){c();return}if(f!==D)return;const F=co(n);if(F){n.preventDefault();let m=l().filter(S=>S.focusable).map(S=>S.ref);switch(F){case"last":{m.reverse();break}case"prev":case"next":{F==="prev"&&m.reverse();const S=m.indexOf(D);m=r.value?po(m,S+1):m.slice(S+1);break}}Qe(()=>{te(m)})}}),h=T(()=>t.value===y(s));return J(we,{rovingFocusGroupItemRef:p,tabIndex:T(()=>y(h)?0:-1),handleMousedown:g,handleFocus:w,handleKeydown:I}),{id:s,handleKeydown:I,handleFocus:w,handleMousedown:g}}});function ho(e,o,t,r,d,c){const l=b("el-roving-focus-collection-item");return N(),L(l,{id:e.id,focusable:e.focusable,active:e.active},{default:v(()=>[P(e.$slots,"default")]),_:3},8,["id","focusable","active"])}var _o=M(Eo,[["render",ho],["__file","/home/runner/work/element-plus/element-plus/packages/components/roving-focus-group/src/roving-focus-item.vue"]]);const j=Symbol("elDropdown"),{ButtonGroup:Co}=ue,yo=A({name:"ElDropdown",components:{ElButton:ue,ElButtonGroup:Co,ElScrollbar:Fe,ElDropdownCollection:Pe,ElTooltip:ye,ElRovingFocusGroup:Io,ElOnlyChild:Te,ElIcon:ce,ArrowDown:ke},props:De,emits:["visible-change","click","command"],setup(e,{emit:o}){const t=me(),r=ee("dropdown"),{t:d}=no(),c=C(),l=C(),s=C(null),p=C(null),g=C(null),w=C(null),I=C(!1),h=[E.enter,E.space,E.down],n=T(()=>({maxHeight:Re(e.maxHeight)})),i=T(()=>[r.m(m.value)]),a=ve().value,f=T(()=>e.id||a);function D(){F()}function F(){var u;(u=s.value)==null||u.onClose()}function B(){var u;(u=s.value)==null||u.onOpen()}const m=Se();function S(...u){o("command",...u)}function $(){}function K(){const u=y(p);u==null||u.focus(),w.value=null}function G(u){w.value=u}function k(u){I.value||(u.preventDefault(),u.stopImmediatePropagation())}function re(){o("visible-change",!0)}function V(u){(u==null?void 0:u.type)==="keydown"&&p.value.focus()}function z(){o("visible-change",!1)}return J(j,{contentRef:p,role:T(()=>e.role),triggerId:f,isUsingKeyboard:I,onItemEnter:$,onItemLeave:K}),J("elDropdown",{instance:t,dropdownSize:m,handleClick:D,commandHandler:S,trigger:Y(e,"trigger"),hideOnClick:Y(e,"hideOnClick")}),{t:d,ns:r,scrollbar:g,wrapStyle:n,dropdownTriggerKls:i,dropdownSize:m,triggerId:f,triggerKeys:h,currentTabId:w,handleCurrentTabIdChange:G,handlerMainButtonClick:u=>{o("click",u)},handleEntryFocus:k,handleClose:F,handleOpen:B,handleBeforeShowTooltip:re,handleShowTooltip:V,handleBeforeHideTooltip:z,onFocusAfterTrapped:u=>{var q,le;u.preventDefault(),(le=(q=p.value)==null?void 0:q.focus)==null||le.call(q,{preventScroll:!0})},popperRef:s,contentRef:p,triggeringElementRef:c,referenceElementRef:l}}});function To(e,o,t,r,d,c){var l;const s=b("el-dropdown-collection"),p=b("el-roving-focus-group"),g=b("el-scrollbar"),w=b("el-only-child"),I=b("el-tooltip"),h=b("el-button"),n=b("arrow-down"),i=b("el-icon"),a=b("el-button-group");return N(),W("div",{class:Z([e.ns.b(),e.ns.is("disabled",e.disabled)])},[R(I,{ref:"popperRef",role:e.role,effect:e.effect,"fallback-placements":["bottom","top"],"popper-options":e.popperOptions,"gpu-acceleration":!1,"hide-after":e.trigger==="hover"?e.hideTimeout:0,"manual-mode":!0,placement:e.placement,"popper-class":[e.ns.e("popper"),e.popperClass],"reference-element":(l=e.referenceElementRef)==null?void 0:l.$el,trigger:e.trigger,"trigger-keys":e.triggerKeys,"trigger-target-el":e.contentRef,"show-after":e.trigger==="hover"?e.showTimeout:0,"stop-popper-mouse-event":!1,"virtual-ref":e.triggeringElementRef,"virtual-triggering":e.splitButton,disabled:e.disabled,transition:`${e.ns.namespace.value}-zoom-in-top`,teleported:"",pure:"",persistent:"",onBeforeShow:e.handleBeforeShowTooltip,onShow:e.handleShowTooltip,onBeforeHide:e.handleBeforeHideTooltip},Xe({content:v(()=>[R(g,{ref:"scrollbar","wrap-style":e.wrapStyle,tag:"div","view-class":e.ns.e("list")},{default:v(()=>[R(p,{loop:e.loop,"current-tab-id":e.currentTabId,orientation:"horizontal",onCurrentTabIdChange:e.handleCurrentTabIdChange,onEntryFocus:e.handleEntryFocus},{default:v(()=>[R(s,null,{default:v(()=>[P(e.$slots,"dropdown")]),_:3})]),_:3},8,["loop","current-tab-id","onCurrentTabIdChange","onEntryFocus"])]),_:3},8,["wrap-style","view-class"])]),_:2},[e.splitButton?void 0:{name:"default",fn:v(()=>[R(w,{id:e.triggerId,role:"button",tabindex:e.tabindex},{default:v(()=>[P(e.$slots,"default")]),_:3},8,["id","tabindex"])])}]),1032,["role","effect","popper-options","hide-after","placement","popper-class","reference-element","trigger","trigger-keys","trigger-target-el","show-after","virtual-ref","virtual-triggering","disabled","transition","onBeforeShow","onShow","onBeforeHide"]),e.splitButton?(N(),L(a,{key:0},{default:v(()=>[R(h,H({ref:"referenceElementRef"},e.buttonProps,{size:e.dropdownSize,type:e.type,disabled:e.disabled,tabindex:e.tabindex,onClick:e.handlerMainButtonClick}),{default:v(()=>[P(e.$slots,"default")]),_:3},16,["size","type","disabled","tabindex","onClick"]),R(h,H({id:e.triggerId,ref:"triggeringElementRef"},e.buttonProps,{role:"button",size:e.dropdownSize,type:e.type,class:e.ns.e("caret-button"),disabled:e.disabled,tabindex:e.tabindex,"aria-label":e.t("el.dropdown.toggleDropdown")}),{default:v(()=>[R(i,{class:Z(e.ns.e("icon"))},{default:v(()=>[R(n)]),_:1},8,["class"])]),_:1},16,["id","size","type","class","disabled","tabindex","aria-label"])]),_:3})):x("v-if",!0)],2)}var Fo=M(yo,[["render",To],["__file","/home/runner/work/element-plus/element-plus/packages/components/dropdown/src/dropdown.vue"]]);const $o=A({name:"DropdownItemImpl",components:{ElIcon:ce},props:fe,emits:["pointermove","pointerleave","click","clickimpl"],setup(e,{emit:o}){const t=ee("dropdown"),{role:r}=_(j,void 0),{collectionItemRef:d}=_(Be,void 0),{collectionItemRef:c}=_(ao,void 0),{rovingFocusGroupItemRef:l,tabIndex:s,handleFocus:p,handleKeydown:g,handleMousedown:w}=_(we,void 0),I=ge(d,c,l),h=T(()=>r.value==="menu"?"menuitem":r.value==="navigation"?"link":"button"),n=O(i=>{const{code:a}=i;if(a===E.enter||a===E.space)return i.preventDefault(),i.stopImmediatePropagation(),o("clickimpl",i),!0},g);return{ns:t,itemRef:I,dataset:{[Ke]:""},role:h,tabIndex:s,handleFocus:p,handleKeydown:n,handleMousedown:w}}}),ko=["aria-disabled","tabindex","role"];function Ro(e,o,t,r,d,c){const l=b("el-icon");return N(),W(eo,null,[e.divided?(N(),W("li",H({key:0,role:"separator",class:e.ns.bem("menu","item","divided")},e.$attrs),null,16)):x("v-if",!0),Ze("li",H({ref:e.itemRef},U(U({},e.dataset),e.$attrs),{"aria-disabled":e.disabled,class:[e.ns.be("menu","item"),e.ns.is("disabled",e.disabled)],tabindex:e.tabIndex,role:e.role,onClick:o[0]||(o[0]=s=>e.$emit("clickimpl",s)),onFocus:o[1]||(o[1]=(...s)=>e.handleFocus&&e.handleFocus(...s)),onKeydown:o[2]||(o[2]=(...s)=>e.handleKeydown&&e.handleKeydown(...s)),onMousedown:o[3]||(o[3]=(...s)=>e.handleMousedown&&e.handleMousedown(...s)),onPointermove:o[4]||(o[4]=s=>e.$emit("pointermove",s)),onPointerleave:o[5]||(o[5]=s=>e.$emit("pointerleave",s))}),[e.icon?(N(),L(l,{key:0},{default:v(()=>[(N(),L(xe(e.icon)))]),_:1})):x("v-if",!0),P(e.$slots,"default")],16,ko)],64)}var So=M($o,[["render",Ro],["__file","/home/runner/work/element-plus/element-plus/packages/components/dropdown/src/dropdown-item-impl.vue"]]);const be=()=>{const e=_("elDropdown",{}),o=T(()=>e==null?void 0:e.dropdownSize);return{elDropdown:e,_elDropdownSize:o}},Oo=A({name:"ElDropdownItem",components:{ElDropdownCollectionItem:Ge,ElRovingFocusItem:_o,ElDropdownItemImpl:So},inheritAttrs:!1,props:fe,emits:["pointermove","pointerleave","click"],setup(e,{emit:o,attrs:t}){const{elDropdown:r}=be(),d=me(),c=C(null),l=T(()=>{var n,i;return(i=(n=y(c))==null?void 0:n.textContent)!=null?i:""}),{onItemEnter:s,onItemLeave:p}=_(j,void 0),g=O(n=>(o("pointermove",n),n.defaultPrevented),ie(n=>{var i;e.disabled?p(n):(s(n),n.defaultPrevented||(i=n.currentTarget)==null||i.focus())})),w=O(n=>(o("pointerleave",n),n.defaultPrevented),ie(n=>{p(n)})),I=O(n=>(o("click",n),n.type!=="keydown"&&n.defaultPrevented),n=>{var i,a,f;if(e.disabled){n.stopImmediatePropagation();return}(i=r==null?void 0:r.hideOnClick)!=null&&i.value&&((a=r.handleClick)==null||a.call(r)),(f=r.commandHandler)==null||f.call(r,e.command,d,n)}),h=T(()=>U(U({},e),t));return{handleClick:I,handlePointerMove:g,handlePointerLeave:w,textContent:l,propsAndAttrs:h}}});function No(e,o,t,r,d,c){var l;const s=b("el-dropdown-item-impl"),p=b("el-roving-focus-item"),g=b("el-dropdown-collection-item");return N(),L(g,{disabled:e.disabled,"text-value":(l=e.textValue)!=null?l:e.textContent},{default:v(()=>[R(p,{focusable:!e.disabled},{default:v(()=>[R(s,H(e.propsAndAttrs,{onPointerleave:e.handlePointerLeave,onPointermove:e.handlePointerMove,onClickimpl:e.handleClick}),{default:v(()=>[P(e.$slots,"default")]),_:3},16,["onPointerleave","onPointermove","onClickimpl"])]),_:3},8,["focusable"])]),_:3},8,["disabled","text-value"])}var Ie=M(Oo,[["render",No],["__file","/home/runner/work/element-plus/element-plus/packages/components/dropdown/src/dropdown-item.vue"]]);const Po=A({name:"ElDropdownMenu",props:Me,setup(e){const o=ee("dropdown"),{_elDropdownSize:t}=be(),r=t.value,{focusTrapRef:d,onKeydown:c}=_(to,void 0),{contentRef:l,role:s,triggerId:p}=_(j,void 0),{collectionRef:g,getItems:w}=_(Ae,void 0),{rovingFocusGroupRef:I,rovingFocusGroupRootStyle:h,tabIndex:n,onBlur:i,onFocus:a,onMousedown:f}=_(ne,void 0),{collectionRef:D}=_(oe,void 0),F=T(()=>[o.b("menu"),o.bm("menu",r==null?void 0:r.value)]),B=ge(l,g,d,I,D),m=O($=>{var K;(K=e.onKeydown)==null||K.call(e,$)},$=>{const{currentTarget:K,code:G,target:k}=$;if(K.contains(k),E.tab===G&&$.stopImmediatePropagation(),$.preventDefault(),k!==y(l)||!Le.includes(G))return;const V=w().filter(z=>!z.disabled).map(z=>z.ref);ze.includes(G)&&V.reverse(),te(V)});return{size:r,rovingFocusGroupRootStyle:h,tabIndex:n,dropdownKls:F,role:s,triggerId:p,dropdownListWrapperRef:B,handleKeydown:$=>{m($),c($)},onBlur:i,onFocus:a,onMousedown:f}}}),Do=["role","aria-labelledby"];function Bo(e,o,t,r,d,c){return N(),W("ul",{ref:e.dropdownListWrapperRef,class:Z(e.dropdownKls),style:oo(e.rovingFocusGroupRootStyle),tabindex:-1,role:e.role,"aria-labelledby":e.triggerId,onBlur:o[0]||(o[0]=(...l)=>e.onBlur&&e.onBlur(...l)),onFocus:o[1]||(o[1]=(...l)=>e.onFocus&&e.onFocus(...l)),onKeydown:o[2]||(o[2]=(...l)=>e.handleKeydown&&e.handleKeydown(...l)),onMousedown:o[3]||(o[3]=(...l)=>e.onMousedown&&e.onMousedown(...l))},[P(e.$slots,"default")],46,Do)}var Ee=M(Po,[["render",Bo],["__file","/home/runner/work/element-plus/element-plus/packages/components/dropdown/src/dropdown-menu.vue"]]);const Qo=Oe(Fo,{DropdownItem:Ie,DropdownMenu:Ee}),Xo=pe(Ie),Zo=pe(Ee);export{Xo as E,Zo as a,Qo as b};