var J=Object.defineProperty;var F=Object.getOwnPropertySymbols;var O=Object.prototype.hasOwnProperty,Q=Object.prototype.propertyIsEnumerable;var P=(l,a,o)=>a in l?J(l,a,{enumerable:!0,configurable:!0,writable:!0,value:o}):l[a]=o,T=(l,a)=>{for(var o in a||(a={}))O.call(a,o)&&P(l,o,a[o]);if(F)for(var o of F(a))Q.call(a,o)&&P(l,o,a[o]);return l};var D=(l,a,o)=>new Promise((c,n)=>{var v=p=>{try{y(o.next(p))}catch(s){n(s)}},u=p=>{try{y(o.throw(p))}catch(s){n(s)}},y=p=>p.done?c(p.value):Promise.resolve(p.value).then(v,u);y((o=o.apply(l,a)).next())});import{_ as W}from"./icon-button.3313e42d.js";import{_ as X}from"./k-table.d4486972.js";import{a as R}from"./replace-all.7cf5f327.js";import{d as S}from"./main.ea42d807.js";import{d as M,g as A,w as Y,o as Z}from"./i18n.48bd28ac.js";import{a as ee,i as oe,r as te}from"./validate.efc4a5c0.js";import{E as le}from"./index.77edae39.js";import{E as ae,a as ne}from"./index.6a6b77f3.js";import{E as se}from"./index.968f79fd.js";import{d as q,c as ie,o as g,j as E,w as m,b as _,f as C,t as $,u as f,a as B,b6 as K,F as L,g as k,b7 as j,aH as U,e as G,k as N}from"./url.2e6a77c4.js";import{E as me}from"./index.fc90f5ad.js";import{E as re,a as ce}from"./index.6855d037.js";import{E as H}from"./index.2bc50276.js";import{E as de}from"./index.c4e9b529.js";import{B as ue}from"./breadcrumb.80c136b8.js";import{b as pe}from"./confirm.e2c924ff.js";import{E as fe}from"./windi.a5b0b048.js";import{c as _e}from"./index.0d2684bf.js";import{E as ve}from"./index.b494e80e.js";import"./index.379e80ad.js";import"./focus-trap.23f44899.js";import"./isNil.98bb3b88.js";import"./event.53b2ad83.js";import"./index.ff0264f3.js";import"./index.8262f5ef.js";import"./index.fcc3ea42.js";import"./sortable.esm.a99254e8.js";import"./index.7034978e.js";import"./isEqual.f1ae9fb3.js";import"./_baseIsEqual.c0d7e77a.js";import"./error.7e8331f1.js";import"./index.695d0284.js";import"./event.776e7e11.js";import"./index.50c16ae5.js";import"./preload-helper.13a99eb0.js";import"./index.d75a71d9.js";import"./index.aba77680.js";import"./index.6d45a031.js";import"./index.31f26a0f.js";import"./index.60af85f4.js";import"./style.19d0c187.js";import"./toNumber.574be4f1.js";import"./dropdown.0507a1c7.js";import"./_baseClone.adbc92f5.js";import"./index.603c1365.js";import"./scroll.f51de5d8.js";import"./refs.4001ce17.js";/* empty css                                                               */import"./plugin-vue_export-helper.21dcd24c.js";import"./plugin-vue_export-helper.41ffa612.js";import"./index.a439b087.js";import"./logo-transparent.1566007e.js";import"./index.99c4f65d.js";import"./aria.75ec5909.js";import"./validator.e2869aba.js";import"./debounce.61c67278.js";import"./index.064c7de9.js";const be=M.global.t,ge=()=>S.get(R("Role/list")),we=l=>S.get(R("Role/isUniqueName"),{name:l},{hiddenError:!0,hiddenLoading:!0}),Ee=l=>S.post(R("Role/Deletes"),{ids:l},void 0,{successMessage:be("common.deleteSuccess")}),ye=M.global.t,Ve=l=>S.get(R("Role/GetEdit"),{name:l}),he=l=>S.post(R("Role/post"),l,void 0,{successMessage:ye("common.saveSuccess")}),ke={class:"text-blue"},$e={class:"text-blue"},xe=q({props:{list:null},setup(l){const a=l,{t:o}=A(),c=ie(()=>a.list?new Set(a.list.map(e=>e.feature)):[]),n=e=>{const t=new Set,r=a.list.filter(i=>i.feature===e);t.add(r.find(i=>i.action==="view")),t.add(r.find(i=>i.action==="edit")),t.add(r.find(i=>i.action==="delete"));for(const i of r)t.add(i);return t.delete(void 0),Array.from(t)},v=e=>e.every(t=>t.access),u=e=>e.some(t=>t.access)&&!v(e),y=e=>{v(e)?e.forEach(t=>t.access=!1):e.forEach(t=>t.access=!0)},p=e=>o(`common.${e}`,e),s=e=>e==="view"?o("permission.view"):o(`common.${e}`,e),d=["edit","delete","debug"],x=(e,t)=>{(t&&d.includes(e.action)||e.action=="view"&&n(e.feature).some(i=>d.includes(i.action)&&i.access))&&h(e.feature)},h=e=>{const t=n(e).find(r=>r.action=="view");t&&(t.access=!0)};return(e,t)=>{const r=le,i=ae,w=se,z=ne;return g(),E(z,null,{default:m(()=>[_(i,{span:6},{default:m(()=>[_(r,{size:"large","model-value":v(l.list),indeterminate:u(l.list),"onUpdate:modelValue":t[0]||(t[0]=b=>y(l.list))},{default:m(()=>[C("span",ke,$(f(o)("common.selectAll")),1)]),_:1},8,["model-value","indeterminate"])]),_:1}),(g(!0),B(L,null,K(f(c),b=>(g(),E(i,{key:b,span:6},{default:m(()=>[n(b).length===1?(g(),E(r,{key:0,modelValue:n(b)[0].access,"onUpdate:modelValue":V=>n(b)[0].access=V,size:"large",label:p(b)},null,8,["modelValue","onUpdate:modelValue","label"])):(g(),E(w,{key:1,placement:"right",trigger:"click"},{reference:m(()=>[_(r,{size:"large",label:p(b),"model-value":v(n(b)),indeterminate:u(n(b))},null,8,["label","model-value","indeterminate"])]),default:m(()=>[C("div",null,[_(r,{size:"large","model-value":v(n(b)),indeterminate:u(n(b)),"onUpdate:modelValue":V=>y(n(b))},{default:m(()=>[C("span",$e,$(f(o)("common.selectAll")),1)]),_:2},1032,["model-value","indeterminate","onUpdate:modelValue"]),(g(!0),B(L,null,K(n(b),V=>(g(),E(r,{key:b+V,modelValue:V.access,"onUpdate:modelValue":[I=>V.access=I,I=>x(V,I)],size:"large",label:s(V.action)},null,8,["modelValue","onUpdate:modelValue","label"]))),128))])]),_:2},1024))]),_:2},1024))),128))]),_:1})}}}),Ce=q({props:{modelValue:{type:Boolean},name:null},emits:["update:modelValue","reload"],setup(l,{emit:a}){const o=l,{t:c}=A(),n=k(!0),v=k(),u=k();Ve(o.name).then(s=>u.value=s);const y={name:o.name?[]:[ee(c("common.roleRequiredTips")),oe(we,c("common.roleExists")),te(1,50)]},p=()=>D(this,null,function*(){yield v.value.validate();const s=T({},u.value);yield he(s),n.value=!1,a("reload")});return(s,d)=>{const x=me,h=re,e=ce,t=H,r=de,i=j("hasPermission");return g(),E(r,{"model-value":n.value,width:"800px","close-on-click-modal":!1,title:l.name?f(c)("common.editRole"):f(c)("common.addRole"),onClosed:d[3]||(d[3]=w=>a("update:modelValue",!1))},{footer:m(()=>[_(t,{round:"","data-cy":"cancel",onClick:d[2]||(d[2]=w=>n.value=!1)},{default:m(()=>[U($(f(c)("common.cancel")),1)]),_:1}),G((g(),E(t,{type:"primary",round:"","data-cy":"save",onClick:p},{default:m(()=>[U($(f(c)("common.save")),1)]),_:1})),[[i,{feature:"role",action:"edit"}]])]),default:m(()=>[u.value?(g(),E(e,{key:0,ref_key:"form",ref:v,"label-position":"top",model:u.value,rules:f(y),onSubmit:d[1]||(d[1]=Y(()=>{},["prevent"])),onKeydown:Z(p,["enter"])},{default:m(()=>[_(h,{label:f(c)("common.roleName"),prop:"name"},{default:m(()=>[_(x,{modelValue:u.value.name,"onUpdate:modelValue":d[0]||(d[0]=w=>u.value.name=w),disabled:!!l.name,"data-cy":"role-name"},null,8,["modelValue","disabled"])]),_:1},8,["label"]),_(h,{label:f(c)("common.permission"),prop:"subItems"},{default:m(()=>[_(xe,{list:u.value.permissions},null,8,["list"])]),_:1},8,["label"])]),_:1},8,["model","rules","onKeydown"])):N("",!0)]),_:1},8,["model-value","title"])}}}),Re={class:"p-24"},Se={class:"flex items-center py-24 space-x-16"},De={class:"flex items-center"},qo=q({setup(l){const{t:a}=A(),o=k(),c=k(""),n=k(!1),v=()=>D(this,null,function*(){o.value=yield ge(),o.value.forEach(s=>{["master","developer","contentmanager"].includes(s.name)&&(s.$DisabledSelect=!0)})}),u=s=>D(this,null,function*(){yield pe(s.length),yield Ee(s.map(d=>d.id)),v()}),y=()=>{c.value="",n.value=!0},p=s=>{c.value=s.name,n.value=!0};return v(),(s,d)=>{const x=fe,h=H,e=_e,t=ve,r=W,i=j("hasPermission");return g(),B("div",Re,[_(ue,{name:f(a)("common.roles")},null,8,["name"]),C("div",Se,[G((g(),E(h,{round:"","data-cy":"add-role",onClick:y},{default:m(()=>[C("div",De,[_(x,{class:"mr-16 iconfont icon-a-addto"}),U(" "+$(f(a)("common.addRole")),1)])]),_:1})),[[i,{feature:"role",action:"edit"}]])]),o.value?(g(),E(f(X),{key:0,data:o.value,sort:"creationDate","show-check":"",permission:{feature:"role",action:"delete"},onDelete:u},{default:m(()=>[_(t,{label:f(a)("common.roleName")},{default:m(({row:w})=>[_(e,{size:"small",class:"rounded-full","data-cy":"name"},{default:m(()=>[U($(w.name),1)]),_:2},1024)]),_:1},8,["label"]),_(t,{align:"right",width:"80px",prop:"creationDate"},{default:m(({row:w})=>[_(r,{icon:"icon-a-writein",tip:f(a)("common.editRole"),"data-cy":"edit",onClick:z=>p(w)},null,8,["tip","onClick"])]),_:1})]),_:1},8,["data"])):N("",!0),n.value?(g(),E(Ce,{key:1,modelValue:n.value,"onUpdate:modelValue":d[0]||(d[0]=w=>n.value=w),name:c.value,onReload:v},null,8,["modelValue","name"])):N("",!0)])}}});export{qo as default};