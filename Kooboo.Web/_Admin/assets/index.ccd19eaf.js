var T=(x,r,n)=>new Promise((d,_)=>{var y=o=>{try{i(n.next(o))}catch(b){_(b)}},m=o=>{try{i(n.throw(o))}catch(b){_(b)}},i=o=>o.done?d(o.value):Promise.resolve(o.value).then(y,m);i((n=n.apply(x,r)).next())});import{S as Y}from"./search-input.67dfc984.js";import{_ as K}from"./k-table.d4486972.js";import{_ as ee}from"./relations-tag.a09361ff.js";import{u as q}from"./use-date.01b82ce0.js";import{g as le,u as oe,m as me,a as pe,b as de,d as te,c as ce,e as ve,i as fe,f as _e}from"./index.288ad073.js";import{_ as ye}from"./boolean-tag.24c364ea.js";import{g as B,o as A,w as H}from"./i18n.48bd28ac.js";import{a as h}from"./validate.efc4a5c0.js";import{_ as Z}from"./index.a731e53b.js";import{E as G}from"./index.fc90f5ad.js";import{E as J,a as Q}from"./index.6855d037.js";import{E as M,a as F,c as ae}from"./index.0d2684bf.js";import{E as W}from"./index.c4e9b529.js";import{d as P,g,c as ne,o as f,j as k,w as u,b as l,u as a,a as U,b6 as D,aH as N,t as E,F as I,f as R,k as S,b7 as be,e as ge,l as ke}from"./url.2e6a77c4.js";import{_ as L}from"./icon-button.3313e42d.js";import{_ as ue}from"./object-type-tag.d88b7d53.js";import{b as se}from"./confirm.e2c924ff.js";import{u as re}from"./page.831d1a45.js";import{E as we}from"./windi.a5b0b048.js";import{E as Ve}from"./index.2bc50276.js";import{E as X}from"./index.b494e80e.js";import{E as $e}from"./main.ea42d807.js";import{B as xe}from"./breadcrumb.80c136b8.js";import{E as Ce,a as Te}from"./index.be352e91.js";/* empty css                                                                 */import"./plugin-vue_export-helper.21dcd24c.js";import"./sortable.esm.a99254e8.js";import"./index.7034978e.js";import"./index.fcc3ea42.js";import"./isEqual.f1ae9fb3.js";import"./_baseIsEqual.c0d7e77a.js";import"./error.7e8331f1.js";import"./index.77edae39.js";import"./index.ff0264f3.js";import"./event.776e7e11.js";import"./index.695d0284.js";import"./replace-all.7cf5f327.js";import"./index.a439b087.js";import"./dayjs.min.0a66969b.js";import"./index.d75a71d9.js";import"./index.aba77680.js";import"./index.6d45a031.js";import"./index.31f26a0f.js";import"./index.60af85f4.js";import"./style.19d0c187.js";import"./toNumber.574be4f1.js";import"./isNil.98bb3b88.js";import"./_baseClone.adbc92f5.js";import"./index.379e80ad.js";import"./focus-trap.23f44899.js";import"./event.53b2ad83.js";import"./scroll.f51de5d8.js";import"./debounce.61c67278.js";import"./index.064c7de9.js";import"./validator.e2869aba.js";import"./index.603c1365.js";import"./refs.4001ce17.js";import"./index.8262f5ef.js";import"./logo-transparent.1566007e.js";import"./index.99c4f65d.js";import"./aria.75ec5909.js";import"./guid.c1a40312.js";import"./index.50c16ae5.js";import"./preload-helper.13a99eb0.js";/* empty css                                                               */import"./plugin-vue_export-helper.41ffa612.js";const Ee={class:"flex flex-wrap gap-4 p-4"},Re=P({props:{modelValue:{type:Boolean},route:null},emits:["update:modelValue","reload"],setup(x,{emit:r}){const n=x,{t:d}=B(),_=g(!0),y=g(),m=g([]);n.route.resourceType=="Route"&&le().then(c=>{m.value=c.sort((s,w)=>s.value.localeCompare(w.value))});const i=g({type:"internal",id:n.route.id,value:n.route.name,destinationObjectId:n.route.objectId}),o=()=>T(this,null,function*(){var c;yield(c=y.value)==null?void 0:c.validate(),yield oe(i.value),_.value=!1,r("reload")}),b=ne(()=>{if(!m.value)return[];const c=m.value.find(s=>s.key==i.value.destinationObjectId);return c?Object.values(c.parameters):[]});return(c,s)=>{const w=G,t=J,v=M,V=F,$=ae,j=Q,C=W;return f(),k(C,{"model-value":_.value,width:"600px","close-on-click-modal":!1,title:a(d)("common.editURL"),onClosed:s[5]||(s[5]=e=>r("update:modelValue",!1))},{footer:u(()=>[l(Z,{permission:{feature:"link",action:"edit"},onConfirm:o,onCancel:s[4]||(s[4]=e=>_.value=!1)})]),default:u(()=>[l(j,{ref_key:"form",ref:y,"label-position":"top",model:i.value,onSubmit:s[3]||(s[3]=H(()=>{},["prevent"]))},{default:u(()=>[l(t,{label:"URL",prop:"value",rules:[a(h)(a(d)("common.urlRequiredTips"))]},{default:u(()=>[l(w,{modelValue:i.value.value,"onUpdate:modelValue":s[0]||(s[0]=e=>i.value.value=e),"data-cy":"url-input",onKeydown:A(o,["enter"]),onInput:s[1]||(s[1]=e=>i.value.value=i.value.value.replace(/\s+/g,""))},null,8,["modelValue","onKeydown"])]),_:1},8,["rules"]),x.route.resourceType=="Route"&&m.value.length?(f(),k(t,{key:0,label:a(d)("common.redirectTo"),prop:"id",rules:[a(h)(a(d)("common.urlRequiredTips"))]},{default:u(()=>[l(V,{modelValue:i.value.destinationObjectId,"onUpdate:modelValue":s[2]||(s[2]=e=>i.value.destinationObjectId=e),class:"w-full",filterable:""},{default:u(()=>[(f(!0),U(I,null,D(m.value,e=>(f(),k(v,{key:e.key,label:e.value,value:e.key},{default:u(()=>[N(E(e.value),1)]),_:2},1032,["label","value"]))),128))]),_:1},8,["modelValue"]),R("div",Ee,[(f(!0),U(I,null,D(a(b),e=>(f(),k($,{key:e,round:""},{default:u(()=>[N(E(e),1)]),_:2},1024))),128))])]),_:1},8,["label","rules"])):S("",!0)]),_:1},8,["model"])]),_:1},8,["model-value","title"])}}}),Ue={class:"flex flex-wrap gap-4 p-4"},ie=P({props:{modelValue:{type:Boolean},url:null},emits:["update:modelValue","reload"],setup(x,{emit:r}){const n=x,{t:d}=B(),_=g(!0),y=g(),m=g([]);le().then(c=>{m.value=c.sort((s,w)=>s.value.localeCompare(w.value))});const i=g({value:n.url,id:""}),o=()=>T(this,null,function*(){var c;yield(c=y.value)==null?void 0:c.validate(),yield me(i.value),_.value=!1,r("reload")}),b=ne(()=>{if(!m.value)return[];const c=m.value.find(s=>s.key==i.value.id);return c?Object.values(c.parameters):[]});return(c,s)=>{const w=G,t=J,v=M,V=F,$=ae,j=Q,C=W;return f(),k(C,{"model-value":_.value,width:"600px","close-on-click-modal":!1,title:a(d)("common.makeAlias"),onClosed:s[5]||(s[5]=e=>r("update:modelValue",!1))},{footer:u(()=>[l(Z,{permission:{feature:"link",action:"edit"},onConfirm:o,onCancel:s[4]||(s[4]=e=>_.value=!1)})]),default:u(()=>[l(j,{ref_key:"form",ref:y,"label-position":"top",model:i.value,onSubmit:s[3]||(s[3]=H(()=>{},["prevent"]))},{default:u(()=>[l(t,{label:"URL",prop:"value",rules:[a(h)(a(d)("common.urlRequiredTips"))]},{default:u(()=>[l(w,{modelValue:i.value.value,"onUpdate:modelValue":s[0]||(s[0]=e=>i.value.value=e),"data-cy":"url-input",onKeydown:A(o,["enter"]),onInput:s[1]||(s[1]=e=>i.value.value=i.value.value.replace(/\s+/g,""))},null,8,["modelValue","onKeydown"])]),_:1},8,["rules"]),l(t,{label:a(d)("common.redirectTo"),prop:"id",rules:[a(h)(a(d)("common.urlRequiredTips"))]},{default:u(()=>[l(V,{modelValue:i.value.id,"onUpdate:modelValue":s[2]||(s[2]=e=>i.value.id=e),class:"w-full",filterable:""},{default:u(()=>[(f(!0),U(I,null,D(m.value,e=>(f(),k(v,{key:e.key,label:e.value,value:e.key},{default:u(()=>[N(E(e.value),1)]),_:2},1032,["label","value"]))),128))]),_:1},8,["modelValue"]),R("div",Ue,[(f(!0),U(I,null,D(a(b),e=>(f(),k($,{key:e,round:""},{default:u(()=>[N(E(e),1)]),_:2},1024))),128))])]),_:1},8,["label","rules"])]),_:1},8,["model"])]),_:1},8,["model-value","title"])}}}),Se={class:"flex space-x-16"},Ie={class:"flex items-center"},je=R("div",{class:"flex-1"},null,-1),Ne=["title"],Oe=P({setup(x){const{t:r}=B(),n=g(),d=g(!1),_=g(!1),y=g(),m=g({type:"",keyword:"",hasObject:void 0}),i=g(),o=w=>T(this,null,function*(){var V;const t=yield pe();i.value=t.resourceType||[];const v={pageNr:w,type:m.value.type,keyword:(V=m.value.keyword)==null?void 0:V.trim(),hasObject:m.value.hasObject,pageSize:30};n.value=yield de(v),n.value.list.forEach($=>{$.hasObject&&$.resourceType!="Route"&&($.$DisabledSelect=!0)})}),b=w=>T(this,null,function*(){var t;yield se(w.length),yield te("internal",w.map(v=>v.id)),o((t=n.value)==null?void 0:t.pageNr)}),{onPreview:c}=re(),s=w=>{y.value=w,d.value=!0};return o(),(w,t)=>{const v=we,V=Ve,$=M,j=F,C=Y,e=X,O=be("hasPermission");return f(),U(I,null,[R("div",Se,[ge((f(),k(V,{round:"",onClick:t[0]||(t[0]=p=>_.value=!0)},{default:u(()=>[R("div",Ie,[l(v,{class:"iconfont icon-a-addto"}),N(" "+E(a(r)("common.makeAlias")),1)])]),_:1})),[[O,{feature:"link",action:"edit"}]]),je,l(j,{modelValue:m.value.type,"onUpdate:modelValue":t[1]||(t[1]=p=>m.value.type=p),placeholder:a(r)("common.resourceType"),clearable:"",class:"w-180px",onChange:t[2]||(t[2]=p=>o(1))},{default:u(()=>[(f(!0),U(I,null,D(i.value,p=>(f(),k($,{key:p.key,label:p.value,value:p.key},null,8,["label","value"]))),128))]),_:1},8,["modelValue","placeholder"]),l(j,{modelValue:m.value.hasObject,"onUpdate:modelValue":t[3]||(t[3]=p=>m.value.hasObject=p),placeholder:a(r)("common.hasObject"),clearable:"",class:"w-140px",onChange:t[4]||(t[4]=p=>o(1))},{default:u(()=>[l($,{label:a(r)("common.yes"),value:!0},null,8,["label"]),l($,{label:a(r)("common.no"),value:!1},null,8,["label"])]),_:1},8,["modelValue","placeholder"]),l(C,{modelValue:m.value.keyword,"onUpdate:modelValue":t[5]||(t[5]=p=>m.value.keyword=p),placeholder:"URL",class:"w-280px h-10",onSearch:t[6]||(t[6]=p=>o(1))},null,8,["modelValue"])]),n.value?(f(),k(a(K),{key:0,class:"mt-24",data:n.value.list,pagination:{currentPage:n.value.pageNr,pageCount:n.value.totalPages,pageSize:n.value.pageSize},"show-check":"",permission:{feature:"link",action:"edit"},onChange:o,onDelete:b},{default:u(()=>[l(e,{label:"URL"},{default:u(({row:p})=>[R("span",{title:p.name,class:"ellipsis","data-cy":"url"},E(p.name),9,Ne)]),_:1}),l(e,{label:a(r)("common.resourceType")},{default:u(({row:p})=>[l(ue,{type:p.resourceType},null,8,["type"])]),_:1},8,["label"]),l(e,{label:a(r)("common.hasObject"),width:"100px",align:"center"},{default:u(({row:p})=>[l(ye,{value:p.hasObject},null,8,["value"])]),_:1},8,["label"]),l(e,{label:a(r)("common.usedBy")},{default:u(({row:p})=>[l(ee,{id:p.id,relations:p.relations,type:"route"},null,8,["id","relations"])]),_:1},8,["label"]),l(e,{label:a(r)("common.lastModified")},{default:u(({row:p})=>[N(E(a(q)(p.lastModified)),1)]),_:1},8,["label"]),l(e,{width:"120px",align:"right"},{default:u(({row:p})=>[l(L,{icon:"icon-a-writein",tip:a(r)("common.edit"),onClick:z=>s(p)},null,8,["tip","onClick"]),l(L,{icon:"icon-eyes",tip:a(r)("common.preview"),onClick:z=>a(c)(p.previewUrl)},null,8,["tip","onClick"])]),_:1})]),_:1},8,["data","pagination"])):S("",!0),d.value?(f(),k(Re,{key:1,modelValue:d.value,"onUpdate:modelValue":t[7]||(t[7]=p=>d.value=p),route:y.value,onReload:t[8]||(t[8]=p=>{var z;return o((z=n.value)==null?void 0:z.pageNr)})},null,8,["modelValue","route"])):S("",!0),_.value?(f(),k(ie,{key:2,modelValue:_.value,"onUpdate:modelValue":t[9]||(t[9]=p=>_.value=p),url:"",onReload:t[10]||(t[10]=p=>{var z;return o((z=n.value)==null?void 0:z.pageNr)})},null,8,["modelValue"])):S("",!0)],64)}}}),ze=R("input",{type:"hidden"},null,-1),De=P({props:{modelValue:{type:Boolean},id:null,value:null},emits:["update:modelValue","reload"],setup(x,{emit:r}){const n=x,{t:d}=B(),_=g(!0),y=g(),m=g({type:"external",id:n.id,value:n.value}),i=()=>T(this,null,function*(){var o;yield(o=y.value)==null?void 0:o.validate(),yield oe(m.value),_.value=!1,r("reload")});return(o,b)=>{const c=G,s=J,w=Q,t=W;return f(),k(t,{"model-value":_.value,width:"600px","close-on-click-modal":!1,title:a(d)("common.editURL"),onClosed:b[4]||(b[4]=v=>r("update:modelValue",!1))},{footer:u(()=>[l(Z,{permission:{feature:"link",action:"edit"},onConfirm:i,onCancel:b[3]||(b[3]=v=>_.value=!1)})]),default:u(()=>[l(w,{ref_key:"form",ref:y,"label-position":"top",model:m.value,onSubmit:b[2]||(b[2]=H(()=>{},["prevent"]))},{default:u(()=>[l(s,{label:"URL",prop:"value",rules:[a(h)(a(d)("common.urlRequiredTips"))]},{default:u(()=>[l(c,{modelValue:m.value.value,"onUpdate:modelValue":b[0]||(b[0]=v=>m.value.value=v),"data-cy":"url-input",onKeydown:A(i,["enter"]),onInput:b[1]||(b[1]=v=>m.value.value=m.value.value.replace(/\s+/g,""))},null,8,["modelValue","onKeydown"]),ze]),_:1},8,["rules"])]),_:1},8,["model"])]),_:1},8,["model-value","title"])}}}),Be={class:"flex space-x-16"},Pe=R("div",{class:"flex-1"},null,-1),Le=["title"],he=P({setup(x){const{t:r}=B(),n=g(),d=g(!1),_=g(),y=g({type:"",keyword:""}),m=g(),i=()=>T(this,null,function*(){const t=yield ce();m.value=t.resourceType||[],yield o()}),o=t=>T(this,null,function*(){var V;const v={pageNr:t,pageSize:30,type:y.value.type,keyword:(V=y.value.keyword)==null?void 0:V.trim()};n.value=yield ve(v)}),b=t=>T(this,null,function*(){yield fe(t),$e.success(r("common.convertedResourceTip")),o()}),c=t=>T(this,null,function*(){var v;yield se(t.length),yield te("external",t.map(V=>V.id)),o((v=n.value)==null?void 0:v.pageNr)}),{onPreview:s}=re(),w=t=>{_.value=t,d.value=!0};return i(),(t,v)=>{const V=M,$=F,j=Y,C=X;return f(),U(I,null,[R("div",Be,[Pe,l($,{modelValue:y.value.type,"onUpdate:modelValue":v[0]||(v[0]=e=>y.value.type=e),placeholder:a(r)("common.resourceType"),clearable:"",class:"w-180px",onChange:v[1]||(v[1]=e=>o(1))},{default:u(()=>[(f(!0),U(I,null,D(m.value,e=>(f(),k(V,{key:e.key,label:e.value,value:e.key},null,8,["label","value"]))),128))]),_:1},8,["modelValue","placeholder"]),l(j,{modelValue:y.value.keyword,"onUpdate:modelValue":v[2]||(v[2]=e=>y.value.keyword=e),placeholder:"URL",class:"w-280px h-10",onSearch:v[3]||(v[3]=e=>o(1))},null,8,["modelValue"])]),n.value?(f(),k(a(K),{key:0,class:"mt-24",data:n.value.list,pagination:{currentPage:n.value.pageNr,pageCount:n.value.totalPages,pageSize:n.value.pageSize},"show-check":"",permission:{feature:"link",action:"delete"},onChange:o,onDelete:c},{default:u(()=>[l(C,{label:"URL"},{default:u(({row:e})=>[R("span",{title:e.name,class:"ellipsis","data-cy":"url"},E(e.name),9,Le)]),_:1}),l(C,{label:a(r)("common.resourceType")},{default:u(({row:e})=>[l(ue,{type:e.resourceType},null,8,["type"])]),_:1},8,["label"]),l(C,{label:a(r)("common.usedBy")},{default:u(({row:e})=>[l(ee,{id:e.id,relations:e.relations,type:"externalResource"},null,8,["id","relations"])]),_:1},8,["label"]),l(C,{label:a(r)("common.lastModified")},{default:u(({row:e})=>[N(E(a(q)(e.lastModified)),1)]),_:1},8,["label"]),l(C,{width:"120px",align:"right"},{default:u(({row:e})=>[l(L,{icon:"icon-a-writein",tip:a(r)("common.edit"),onClick:O=>w(e)},null,8,["tip","onClick"]),["Script","Style","Image"].includes(e.resourceType)?(f(),k(L,{key:0,icon:"icon-xiazai-wenjianxiazai-05",tip:a(r)("common.internalizeResource"),"data-cy":"internalizeResource",onClick:O=>b(e.id)},null,8,["tip","onClick"])):S("",!0),l(L,{icon:"icon-eyes",tip:a(r)("common.preview"),"data-cy":"preview",onClick:O=>a(s)(e.previewUrl)},null,8,["tip","onClick"])]),_:1})]),_:1},8,["data","pagination"])):S("",!0),d.value?(f(),k(De,{key:1,id:_.value.id,modelValue:d.value,"onUpdate:modelValue":v[4]||(v[4]=e=>d.value=e),type:"external",value:_.value.name,onReload:v[5]||(v[5]=e=>{var O;return o((O=n.value)==null?void 0:O.pageNr)})},null,8,["id","modelValue","value"])):S("",!0)],64)}}}),Me=["title"],Fe=P({setup(x){const{t:r}=B(),n=g(),d=g(!1),_=g(),y=i=>T(this,null,function*(){n.value=yield _e({pageNr:i,pageSize:30})}),m=i=>{_.value=i,d.value=!0};return y(),(i,o)=>{const b=X;return f(),U(I,null,[n.value?(f(),k(a(K),{key:0,class:"mt-24",data:n.value.list,pagination:{currentPage:n.value.pageNr,pageCount:n.value.totalPages,pageSize:n.value.pageSize},onChange:y},{default:u(()=>[l(b,{label:"URL"},{default:u(({row:c})=>[R("span",{title:c.url,class:"ellipsis","data-cy":"url"},E(c.url),9,Me)]),_:1}),l(b,{label:a(r)("common.dateTime")},{default:u(({row:c})=>[N(E(a(q)(c.startTime+"Z")),1)]),_:1},8,["label"]),l(b,{width:"120px",align:"right"},{default:u(({row:c})=>[l(L,{permission:{feature:"link",action:"edit"},icon:"icon-copy",tip:a(r)("common.makeAlias"),onClick:s=>m(c)},null,8,["tip","onClick"])]),_:1})]),_:1},8,["data","pagination"])):S("",!0),d.value?(f(),k(ie,{key:1,modelValue:d.value,"onUpdate:modelValue":o[0]||(o[0]=c=>d.value=c),url:_.value.url,onReload:o[1]||(o[1]=c=>y())},null,8,["modelValue","url"])):S("",!0)],64)}}}),oo=P({setup(x){const{t:r}=B(),n=[{displayName:r("common.internal"),value:"internal",component:Oe},{displayName:r("common.external"),value:"external",component:he},{displayName:r("common.notFound"),value:"notFound",component:Fe}],d=g(n[0].value);return(_,y)=>{const m=Ce,i=Te;return f(),U("div",null,[l(xe,{name:a(r)("common.urls"),class:"p-24"},null,8,["name"]),l(i,{modelValue:d.value,"onUpdate:modelValue":y[0]||(y[0]=o=>d.value=o)},{default:u(()=>[(f(),U(I,null,D(n,o=>l(m,{key:o.value,label:o.displayName,name:o.value},{default:u(()=>[d.value===o.value?(f(),k(ke(o.component),{key:0})):S("",!0)]),_:2},1032,["label","name"])),64))]),_:1},8,["modelValue"])])}}});export{oo as default};